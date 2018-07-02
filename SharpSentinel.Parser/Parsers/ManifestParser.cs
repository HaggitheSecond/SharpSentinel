using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using JetBrains.Annotations;
using SharpSentinel.Parser.Data;
using SharpSentinel.Parser.Data.Internal;
using SharpSentinel.Parser.Data.ManifestObjects;
using SharpSentinel.Parser.Data.S1;
using SharpSentinel.Parser.Extensions;
using SharpSentinel.Parser.Helpers;
// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute

namespace SharpSentinel.Parser.Parsers
{
    internal static class ManifestParser
    {
        /// <summary>
        /// Parses the manifest.safe data
        /// </summary>
        /// <param name="directory">Path to the SAFE directory</param>
        /// <returns>The parsed manifest data</returns>
        public static S1Data Parse([NotNull]string directory)
        {
            Guard.NotNullOrWhitespace(directory, nameof(directory));

            var directoryInfo = new DirectoryInfo(directory);

            Guard.NotNullAndValidFileSystemInfo(directoryInfo, nameof(directoryInfo));

            var data = new S1Data
            {
                BaseDirectory = directoryInfo
            };

            var file = FileHelper.GetFiles(directory, SAFEFileTypes.Manifest).First();

            Guard.NotNullAndValidFileSystemInfo(file, nameof(file));

            using (var fileStream = new FileStream(file.FullName, FileMode.Open))
            {
                var document = new XmlDocument();
                document.Load(fileStream);

                var manager = GenerateManager(document);

                var informationPackageMap = document.SelectSingleNodeThrowIfNull("xfdu:XFDU/informationPackageMap", manager);
                var metaDataSection = document.SelectSingleNodeThrowIfNull("xfdu:XFDU/metadataSection", manager);
                var dataObjectSection = document.SelectSingleNodeThrowIfNull("xfdu:XFDU/dataObjectSection", manager);
                
                data.Manifest = new Manifest
                {
                    MetaData = MetaDataParser.Parse(metaDataSection, manager),
                    RawXML = document.InnerXml,
                    File = file
                };

                var allDataUnits = MeasurementDataUnitParser.Parse(informationPackageMap, metaDataSection, dataObjectSection, manager, data.BaseDirectory);
                data.MeasurementDataUnits = allDataUnits.Where(f => f.MeasurementDataUnitType == MeasurementDataUnitType.Measurement).ToList();
                data.QuickLookDataUnit = allDataUnits.FirstOrDefault(f => f.MeasurementDataUnitType == MeasurementDataUnitType.QuickLook);
            }

            var reportFile = directoryInfo.GetFiles().FirstOrDefault(f => f.Name.Contains("report") && f.Extension == ".pdf");
            if (reportFile != null)
                data.ReportFile = reportFile;

            return data;
        }

        private static XmlNamespaceManager GenerateManager([NotNull]XmlDocument document)
        {
            Guard.NotNull(document, nameof(document));

            var manager = new XmlNamespaceManager(document.NameTable);

            manager.AddNamespace("safe", "http://www.esa.int/safe/sentinel-1.0");
            manager.AddNamespace("s1", "http://www.esa.int/safe/sentinel-1.0/sentinel-1");
            manager.AddNamespace("s1sarl1", "http://www.esa.int/safe/sentinel-1.0/sentinel-1/sar/level-1");
            manager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            manager.AddNamespace("xfdu", "urn:ccsds:schema:xfdu:1");

            return manager;
        }
    }
}