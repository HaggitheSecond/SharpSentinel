using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using SharpSentinel.Parser.Data;
using SharpSentinel.Parser.Data.Internal;
using SharpSentinel.Parser.Data.ManifestObjects;
using SharpSentinel.Parser.Data.S1;
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
        public static S1Data Parse(string directory)
        {
            var data = new S1Data
            {
                BaseDirectory = new DirectoryInfo(directory)
            };

            var file = FileHelper.GetFiles(directory, SAFEFileTypes.Manifest).First();

            using (var fileStream = new FileStream(file.FullName, FileMode.Open))
            {
                var document = new XmlDocument();
                document.Load(fileStream);

                var manager = GenerateManager(document);

                var informationPackageMap = document.SelectSingleNode("xfdu:XFDU/informationPackageMap", manager);
                if (informationPackageMap == null)
                    throw new XmlException("Missing informationPackageMap in manifest.safe");

                var metaDataSection = document.SelectSingleNode("xfdu:XFDU/metadataSection", manager);
                if (metaDataSection == null)
                    throw new XmlException("Missing metadataSection in manifest.safe");

                var dataObjectSection = document.SelectSingleNode("xfdu:XFDU/dataObjectSection", manager);
                if (dataObjectSection == null)
                    throw new XmlException("Missing dataObjectSection in manifest.safe");

                data.Manifest = new Manifest
                {
                    MetaData = MetaDataParser.Parse(metaDataSection, manager),
                    RawXML = document.InnerXml,
                    File = file
                };

                var allDataUnits = MeasurementDataUnitParser.Parse(informationPackageMap, dataObjectSection, metaDataSection, manager, data.BaseDirectory);
                data.MeasurementDataUnits = allDataUnits.Where(f => f.MeasurementDataUnitType == MeasurementDataUnitType.Measurement).ToList();
                data.QuickLookDataUnit = allDataUnits.FirstOrDefault(f => f.MeasurementDataUnitType == MeasurementDataUnitType.QuickLook);
            }

            return data;
        }

        private static XmlNamespaceManager GenerateManager(XmlDocument document)
        {
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