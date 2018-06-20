using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using SharpSentinel.Parser.Data.Internal;
using SharpSentinel.Parser.Data.ManifestObjects;
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
        public static Manifest Parse(string directory)
        {
            var file = FileHelper.GetFiles(directory, SAFEFileTypes.Manifest).First();

            using (var fileStream = new FileStream(file.FullName, FileMode.Open))
            {
                var document = new XmlDocument();
                document.Load(fileStream);

                var manifest = new Manifest(file.FullName);

                var manager = GenerateManager(document);

                var xfduSection = document.ChildNodes[1];
                var metaDataSection = xfduSection.SelectSingleNode("metadataSection");

                if (metaDataSection == null)
                    throw new XmlException();

                var acqNode = metaDataSection.SelectMetaDataObjectByID("acquisitionPeriod");
                manifest.AcquisitionPeriod = AcquisitionPeriodParser.Parse(acqNode, manager);

                var gpiNode = metaDataSection.SelectMetaDataObjectByID("generalProductInformation");
                manifest.GeneralProductInformation = GeneralProductInformationParser.Parse(gpiNode, manager);

                var mfsNode = metaDataSection.SelectMetaDataObjectByID("measurementFrameSet");
                if (mfsNode != null)
                    manifest.MeasurementFrameSet = MeasurementFrameParser.Parse(mfsNode, manager);

                var morNode = metaDataSection.SelectMetaDataObjectByID("measurementOrbitReference");
                manifest.MeasurementOrbitReference = MeasurementOrbitReferenceParser.Parse(morNode, manager);

                var platformNode = metaDataSection.SelectMetaDataObjectByID("platform");
                manifest.Platform = PlatformParser.Parse(platformNode, manager);

                var processingNode = metaDataSection.SelectMetaDataObjectByID("processing");
                manifest.Processing = ProcessingParser.Parse(processingNode, manager);

                return manifest;
            }
        }

        public static class ProcessingParser
        {
            public static Processing Parse(XmlNode node, XmlNamespaceManager manager)
            {
                var dataNode = node.SelectSingleNode("metadataWrap/xmlData/safe:processing", manager);
                return ParseProcessing(dataNode, manager);
            }

            private static Processing ParseProcessing(XmlNode node, XmlNamespaceManager manager)
            {
                var processing = new Processing();

                foreach (var attribute in node.Attributes.Cast<XmlAttribute>())
                {
                    switch (attribute.Name)
                    {
                        case "name":
                            processing.Name = attribute.Value;
                            break;
                        case "start":
                            processing.Start = DateTimeOffset.Parse(attribute.Value);
                            break;
                        case "stop":
                            processing.Stop = DateTimeOffset.Parse(attribute.Value);
                            break;
                    }
                }

                var facilityNode = node.SelectSingleNode("safe:facility", manager);
                if (facilityNode != null)
                    processing.Facility = ParseFacility(facilityNode, manager);

                var resourceNodes = node.SelectNodes("safe:resource", manager);
                foreach (var currentResourceNode in resourceNodes.Cast<XmlNode>())
                {
                    processing.Resources.Add(ParseResource(currentResourceNode, manager));
                }

                return processing;
            }

            private static ProcessingResource ParseResource(XmlNode node, XmlNamespaceManager manager)
            {
                var resource = new ProcessingResource();

                foreach (var attribute in node.Attributes.Cast<XmlAttribute>())
                {
                    switch (attribute.Name)
                    {
                        case "name":
                            resource.Name = attribute.Value;
                            break;
                        case "role":
                            resource.Role = attribute.Value;
                            break;
                        case "href":
                            resource.Href = new Uri(attribute.Value);
                            break;
                    }
                }

                var processingNodes = node.SelectNodes("safe:processing", manager);

                foreach (var currentProcessingNode in processingNodes.Cast<XmlNode>())
                {
                    resource.Processing = ParseProcessing(currentProcessingNode, manager);
                }

                return resource;
            }

            private static ProcessingFacility ParseFacility(XmlNode node, XmlNamespaceManager manager)
            {
                var facility = new ProcessingFacility();

                foreach (var attribute in node.Attributes.Cast<XmlAttribute>())
                {
                    switch (attribute.Name)
                    {
                        case "name":
                            facility.Name = attribute.Value;
                            break;
                        case "country":
                            facility.Country = attribute.Value;
                            break;
                        case "organisation":
                            facility.Organisation = attribute.Value;
                            break;
                        case "site":
                            facility.Site = attribute.Value;
                            break;
                    }
                }

                var softwareNodes = node.SelectNodes("safe:software", manager);

                foreach (var currentSoftwareNode in softwareNodes.Cast<XmlNode>())
                {
                    var software = new ProcessingSoftware();

                    foreach (var attribute in currentSoftwareNode.Attributes.Cast<XmlAttribute>())
                    {
                        switch (attribute.Name)
                        {
                            case "name":
                                software.Name = attribute.Value;
                                break;
                            case "version":
                                software.Version = attribute.Value;
                                break;
                        }
                    }

                    facility.Software.Add(software);
                }

                return facility;
            }

        }

        public static class PlatformParser
        {
            public static Platform Parse(XmlNode node, XmlNamespaceManager manager)
            {
                var platform = new Platform { Instrument = new PlatformInstrument(), LeapSecondInformation = new LeapSecondInformation() };

                var dataNode = node.SelectSingleNode("metadataWrap/xmlData/safe:platform", manager);

                var nssdcIdentifierNode = dataNode.SelectSingleNode("safe:nssdcIdentifier", manager);
                platform.NssdcIdentifier = nssdcIdentifierNode.InnerText;

                var familyNameNode = dataNode.SelectSingleNode("safe:familyName", manager);
                platform.FamilyName = familyNameNode.InnerText;

                var numberNode = dataNode.SelectSingleNode("safe:number", manager);
                platform.Number = numberNode.InnerText;

                var instrumentNode = dataNode.SelectSingleNode("safe:instrument", manager);

                var instrumentFamilyNameNode = instrumentNode.SelectSingleNode("safe:familyName", manager);
                platform.Instrument.Name = instrumentFamilyNameNode.InnerText;
                platform.Instrument.Abbreviation = instrumentFamilyNameNode.Attributes[0].InnerText;

                var instrumentModeNode = instrumentNode.SelectSingleNode("safe:extension/s1sarl1:instrumentMode/s1sarl1:mode", manager);
                platform.Instrument.Mode = (InstrumentModeType)Enum.Parse(typeof(InstrumentModeType), instrumentModeNode.InnerText);

                var swathNodes = instrumentNode.SelectNodes("safe:extension/s1sarl1:instrumentMode/s1sarl1:swath", manager);

                foreach (var currentSwathNode in swathNodes.Cast<XmlNode>())
                {
                    platform.Instrument.Swaths.Add((SwathType)Enum.Parse(typeof(SwathType), currentSwathNode.InnerText));
                }

                var extensionNode = dataNode.SelectSingleNode("safe:extension", manager);

                var leapSecondInformationNode = extensionNode?.SelectSingleNode("s1:leapSecondInformation", manager);

                if (leapSecondInformationNode != null)
                {
                    var utcTimeOfOccurrenceNode = leapSecondInformationNode.SelectSingleNode("s1:utcTimeOfOccurrence", manager);
                    platform.LeapSecondInformation.UtcTimeOfOccurence = DateTimeOffset.Parse(utcTimeOfOccurrenceNode.InnerText);

                    var signNode = leapSecondInformationNode.SelectSingleNode("s1:sign", manager);

                    switch (signNode.InnerText)
                    {
                        case "+":
                            platform.LeapSecondInformation.Sign = SignType.Plus;
                            break;
                        case "-":
                            platform.LeapSecondInformation.Sign = SignType.Minus;
                            break;
                    }
                }

                return platform;
            }
        }

        public static class MeasurementOrbitReferenceParser
        {
            public static MeasurementOrbitReference Parse(XmlNode node, XmlNamespaceManager manager)
            {
                var measurementOrbitReference = new MeasurementOrbitReference();

                var dataNode = node.SelectSingleNode("metadataWrap/xmlData/safe:orbitReference", manager);

                var orbitNumberNodes = dataNode.SelectNodes("safe:orbitNumber", manager);

                for (var i = 0; i < 2; i++)
                {
                    var attributes = orbitNumberNodes[i].Attributes;

                    switch (attributes[0].Value)
                    {
                        case "start":
                            measurementOrbitReference.OrbitNumberStart = int.Parse(orbitNumberNodes[i].InnerText);
                            break;
                        case "stop":
                            measurementOrbitReference.OrbitNumberStop = int.Parse(orbitNumberNodes[i].InnerText);
                            break;
                        default:
                            throw new XmlException();
                    }
                }

                var relativeOrbitNumberNodes = dataNode.SelectNodes("safe:relativeOrbitNumber", manager);

                for (var i = 0; i < 2; i++)
                {
                    var attributes = relativeOrbitNumberNodes[i].Attributes;

                    switch (attributes[0].Value)
                    {
                        case "start":
                            measurementOrbitReference.RelativeOrbitNumberStart = int.Parse(relativeOrbitNumberNodes[i].InnerText);
                            break;
                        case "stop":
                            measurementOrbitReference.RelativeOrbitNumberStop = int.Parse(relativeOrbitNumberNodes[i].InnerText);
                            break;
                        default:
                            throw new XmlException();
                    }
                }

                var cycleNumberNode = dataNode.SelectSingleNode("safe:cycleNumber", manager);
                measurementOrbitReference.CycleNumber = int.Parse(cycleNumberNode.InnerText);

                var phaseIdentifierNode = dataNode.SelectSingleNode("safe:phaseIdentifier", manager);
                measurementOrbitReference.PhaseIdentifier = int.Parse(phaseIdentifierNode.InnerText);

                var passNode = dataNode.SelectSingleNode("safe:extension/s1:orbitProperties/s1:pass", manager);
                measurementOrbitReference.Pass = (PassType)Enum.Parse(typeof(PassType), passNode.InnerText);

                var ascendingNodeTimeNode = dataNode.SelectSingleNode("safe:extension/s1:orbitProperties/s1:ascendingNodeTime", manager);

                if (ascendingNodeTimeNode != null)
                    measurementOrbitReference.AscendingNodeTime = DateTimeOffset.Parse(ascendingNodeTimeNode.InnerText);

                return measurementOrbitReference;
            }
        }

        public static class MeasurementFrameParser
        {
            public static MeasurementFrameSet Parse(XmlNode node, XmlNamespaceManager manager)
            {
                var measureementFrameSet = new MeasurementFrameSet();

                var dataNodes = node.SelectNodes("metadataWrap/xmlData/safe:frameSet/safe:frame", manager);

                foreach (var currentFrameNode in dataNodes.Cast<XmlNode>())
                {
                    measureementFrameSet.MeasurementFrames.Add(ParseFrame(currentFrameNode, manager));
                }

                return measureementFrameSet;
            }

            private static MeasurementFrame ParseFrame(XmlNode node, XmlNamespaceManager manager)
            {
                var measurementFrame = new MeasurementFrame();

                var numberNode = node.SelectSingleNode("safe:number", manager);

                if (numberNode != null)
                    measurementFrame.Number = int.Parse(numberNode.InnerText);

                var footPrintNode = node.SelectSingleNode("safe:footPrint", manager);
                measurementFrame.Footprint = footPrintNode.InnerText;

                return measurementFrame;
            }
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

                var transmitterReceiverPolarisationNodes = dataNode.SelectNodes("s1sarl1:transmitterReceiverPolarisation", manager);
                foreach (var transmitterReceiverPolarisationNode in transmitterReceiverPolarisationNodes.Cast<XmlNode>())
                {
                    generalProductInformation.TransmitterReceiverPolarisation.Add((TransmitterReceiverPolarisationType)Enum.Parse(typeof(TransmitterReceiverPolarisationType), transmitterReceiverPolarisationNode.InnerText));
                }

                var productClassNode = dataNode.SelectSingleNode("s1sarl1:productClass", manager);
                generalProductInformation.ProductClass = (ProductClassType)Enum.Parse(typeof(ProductClassType), productClassNode.InnerText);

                var productClassDescriptionNode = dataNode.SelectSingleNode("s1sarl1:productClassDescription", manager);
                generalProductInformation.ProductClassDescription = (ProductClassDescriptionType)Enum.Parse(typeof(ProductClassDescriptionType), productClassDescriptionNode.InnerText.RemoveWhitespaces());

                var productCompositionNode = dataNode.SelectSingleNode("s1sarl1:productComposition", manager);
                generalProductInformation.ProductComposition = (ProductCompositionType)Enum.Parse(typeof(ProductCompositionType), productCompositionNode.InnerText);

                var productTypeNode = dataNode.SelectSingleNode("s1sarl1:productType", manager);
                generalProductInformation.ProductType = (ProductTypeType)Enum.Parse(typeof(ProductTypeType), productTypeNode.InnerText);

                var productTimelinessCategoryNode = dataNode.SelectSingleNode("s1sarl1:productTimelinessCategory", manager);
                generalProductInformation.ProductTimelinessCategory = (ProductTimelinessCategoryType)Enum.Parse(typeof(ProductTimelinessCategoryType), productTimelinessCategoryNode.InnerText.Replace("-", ""));

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

        private static XmlNamespaceManager GenerateManager(XmlDocument document)
        {
            var manager = new XmlNamespaceManager(document.NameTable);
            manager.AddNamespace("safe", "http://www.esa.int/safe/sentinel-1.0");
            manager.AddNamespace("s1", "http://www.esa.int/safe/sentinel-1.0/sentinel-1");
            manager.AddNamespace("s1sarl1", "http://www.esa.int/safe/sentinel-1.0/sentinel-1/sar/level-1");

            return manager;
        }
    }
}