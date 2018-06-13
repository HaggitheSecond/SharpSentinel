using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using SharpSentinel.Parser.Data.Internal;
using SharpSentinel.Parser.Data.ManifestObjects;
using SharpSentinel.Parser.Extensions;
using SharpSentinel.Parser.Helpers;
// ReSharper disable PossibleNullReferenceException

namespace SharpSentinel.Parser.Parsers
{
    internal static class ManifestParser
    {
        /// <summary>
        /// Parses the manifest.safe data
        /// </summary>
        /// <param name="directory">Path to the SAFE directory</param>
        /// <returns>The parsed manifest data</returns>
        public static Manifest Parse(string directory)
        {
            var file = FileHelper.GetFiles(directory, SAFEFileTypes.Manifest).First();

            using (var fileStream = new FileStream(file.FullName, FileMode.Open))
            {
                var document = new XmlDocument();
                document.Load(fileStream);

                var manifest = new Manifest(file.FullName);

                var manager = GenerateManger(document);

                var xfduSection = document.ChildNodes[1];
                var metaDataSection = xfduSection.SelectSingleNode("metadataSection");

                if (metaDataSection == null)
                    throw new XmlException();

                var acqNode = metaDataSection.SelectMetaDataObjectByID("acquisitionPeriod");
                manifest.AcquisitionPeriod = AcquisitionPeriodParser.Parse(acqNode, manager);

                var gpiNode = metaDataSection.SelectMetaDataObjectByID("generalProductInformation");
                manifest.GeneralProductInformation = GeneralProductInformationParser.Parse(gpiNode, manager);

                return manifest;
            }
        }

        private static XmlNamespaceManager GenerateManger(XmlDocument document)
        {
            var manager = new XmlNamespaceManager(document.NameTable);
            manager.AddNamespace("safe", "http://www.esa.int/safe/sentinel-1.0");
            manager.AddNamespace("s1", "http://www.esa.int/safe/sentinel-1.0/sentinel-1");
            manager.AddNamespace("s1sarl1", "http://www.esa.int/safe/sentinel-1.0/sentinel-1/sar/level-1");

            return manager;
        }

        public static class AcquisitionPeriodParser
        {
            public static AcquisitionPeriod Parse(XmlNode node, XmlNamespaceManager manager)
            {
                var acquisitionPerid = new AcquisitionPeriod();

                var dataNode = node.SelectSingleNode("metadataWrap/xmlData/safe:acquisitionPeriod", manager);

                var startTimeNode = dataNode.SelectSingleNode("safe:startTime", manager);
                acquisitionPerid.StartTime = DateTimeOffset.Parse(startTimeNode.InnerText);
                var stopTimeNode = dataNode.SelectSingleNode("safe:stopTime", manager);
                acquisitionPerid.StopTime = DateTimeOffset.Parse(stopTimeNode.InnerText);

                var timeAnxNode = dataNode.SelectSingleNode("safe:extension/s1:timeANX", manager);

                var startTimeANXNode = timeAnxNode.SelectSingleNode("s1:startTimeANX", manager);
                acquisitionPerid.StartTimeANX = float.Parse(startTimeANXNode.InnerText);
                var stopTimeANXNode = timeAnxNode.SelectSingleNode("s1:stopTimeANX", manager);
                acquisitionPerid.StopTimeANX = float.Parse(stopTimeANXNode.InnerText);

                return acquisitionPerid;
            }
        }

        public static class GeneralProductInformationParser
        {
            public static GeneralProductInformation Parse(XmlNode node, XmlNamespaceManager manager)
            {
                var generalProductInformation = new GeneralProductInformation();

                var dataNode = node.SelectSingleNode("metadataWrap/xmlData/s1sarl1:standAloneProductInformation", manager);

                var instrumentConfigurationIDNode = dataNode.SelectSingleNode("s1sarl1:instrumentConfigurationID", manager);
                generalProductInformation.InstrumentConfigurationID = int.Parse(instrumentConfigurationIDNode.InnerText);

                var missionDataTakeIDNode = dataNode.SelectSingleNode("s1sarl1:missionDataTakeID", manager);
                generalProductInformation.MissionDataTakeID = int.Parse(missionDataTakeIDNode.InnerText);

                generalProductInformation.TransmitterReceiverPolarisation = new List<TransmitterReceiverPolarisationType>();
                var transmitterReceiverPolarisationNodes = dataNode.SelectNodes("s1sarl1:transmitterReceiverPolarisation", manager);
                foreach (var transmitterReceiverPolarisationNode in transmitterReceiverPolarisationNodes.Cast<XmlNode>())
                {
                    if(Enum.TryParse(transmitterReceiverPolarisationNode.InnerText, out TransmitterReceiverPolarisationType type) == false)
                        throw new InvalidCastException();
                    generalProductInformation.TransmitterReceiverPolarisation.Add(type);
                }

                var productClassNode = dataNode.SelectSingleNode("s1sarl1:productClass", manager);
                if (Enum.TryParse(productClassNode.InnerText, out ProductClassType productClassType) == false)
                    throw new InvalidCastException();
                generalProductInformation.ProductClass = productClassType;

                var productClassDescriptionNode = dataNode.SelectSingleNode("s1sarl1:productClassDescription", manager);
                if (Enum.TryParse(productClassDescriptionNode.InnerText.RemoveWhitespaces(), out ProductClassDescriptionType productClassDescriptionType) == false)
                    throw new InvalidCastException();
                generalProductInformation.ProductClassDescription = productClassDescriptionType;

                var productCompositionNode = dataNode.SelectSingleNode("s1sarl1:productComposition", manager);
                if (Enum.TryParse(productCompositionNode.InnerText, out ProductCompositionType productCompositionType) == false)
                    throw new InvalidCastException();
                generalProductInformation.ProductComposition = productCompositionType;

                var productTypeNode = dataNode.SelectSingleNode("s1sarl1:productType", manager);
                if (Enum.TryParse(productTypeNode.InnerText, out ProductTypeType productTypeType) == false)
                    throw new InvalidCastException();
                generalProductInformation.ProductType = productTypeType;

                var productTimelinessCategoryNode = dataNode.SelectSingleNode("s1sarl1:productTimelinessCategory", manager);
                if (Enum.TryParse(productTimelinessCategoryNode.InnerText.Replace("-", ""), out ProductTimelinessCategoryType productTimelinessCategoryType) == false)
                    throw new InvalidCastException();
                generalProductInformation.ProductTimelinessCategory = productTimelinessCategoryType;

                var sliceProductFlagNode = dataNode.SelectSingleNode("s1sarl1:sliceProductFlag", manager);
                generalProductInformation.SlideProductFlag = bool.Parse(sliceProductFlagNode.InnerText);

                if (generalProductInformation.SlideProductFlag)
                {
                    var segmentStartTimeNode = dataNode.SelectSingleNode("s1sarl1:segmentStartTime", manager);
                    generalProductInformation.SegementStartTime = DateTimeOffset.Parse(segmentStartTimeNode.InnerText);

                    var sliceNumberNode = dataNode.SelectSingleNode("s1sarl1:sliceNumber", manager);
                    generalProductInformation.SliceNumber = int.Parse(sliceNumberNode.InnerText);

                    var totalSlicesNode = dataNode.SelectSingleNode("s1sarl1:totalSlices", manager);
                    generalProductInformation.TotalSlices = int.Parse(totalSlicesNode.InnerText);
                }

                return generalProductInformation;
            }
        }
    }

    public static class XMLExtensions
    {
        public static XmlNode SelectMetaDataObjectByID(this XmlNode self, string id)
        {
            return self.SelectSingleNode($"metadataObject[@ID='{id}']");
        }
    }
}