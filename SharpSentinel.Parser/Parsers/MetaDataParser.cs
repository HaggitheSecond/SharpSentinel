using System;
using System.Linq;
using System.Xml;
using JetBrains.Annotations;
using SharpSentinel.Parser.Data.ManifestObjects;
using SharpSentinel.Parser.Extensions;
using SharpSentinel.Parser.Helpers;
// ReSharper disable MemberHidesStaticFromOuterClass
// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute

namespace SharpSentinel.Parser.Parsers
{
    public static class MetaDataParser
    {
        public static MetaData Parse([NotNull]XmlNode metaDataNode, [NotNull]XmlNamespaceManager manager)
        {
            Guard.NotNull(metaDataNode, nameof(metaDataNode));
            Guard.NotNull(manager, nameof(manager));

            var manifest = new MetaData();

            var acqNode = metaDataNode.SelectMetaDataObjectByID("acquisitionPeriod");
            manifest.AcquisitionPeriod = AcquisitionPeriodParser.Parse(acqNode, manager);

            var gpiNode = metaDataNode.SelectMetaDataObjectByID("generalProductInformation");
            manifest.GeneralProductInformation = GeneralProductInformationParser.Parse(gpiNode, manager);

            var mfsNode = metaDataNode.SelectMetaDataObjectByID("measurementFrameSet");
            if (mfsNode != null)
                manifest.MeasurementFrameSet = MeasurementFrameParser.Parse(mfsNode, manager);

            var morNode = metaDataNode.SelectMetaDataObjectByID("measurementOrbitReference");
            manifest.MeasurementOrbitReference = MeasurementOrbitReferenceParser.Parse(morNode, manager);

            var platformNode = metaDataNode.SelectMetaDataObjectByID("platform");
            manifest.Platform = PlatformParser.Parse(platformNode, manager);

            var processingNode = metaDataNode.SelectMetaDataObjectByID("processing");
            manifest.Processing = ProcessingParser.Parse(processingNode, manager);

            return manifest;
        }

        private static class ProcessingParser
        {
            public static Processing Parse([NotNull]XmlNode processingNode, [NotNull]XmlNamespaceManager manager)
            {
                Guard.NotNull(processingNode, nameof(processingNode));
                Guard.NotNull(manager, nameof(manager));

                var dataNode = processingNode.SelectSingleNodeThrowIfNull("metadataWrap/xmlData/safe:processing", manager);
                return ParseProcessing(dataNode, manager);
            }

            private static Processing ParseProcessing([NotNull]XmlNode processingNode, [NotNull]XmlNamespaceManager manager)
            {
                Guard.NotNull(processingNode, nameof(processingNode));
                Guard.NotNull(manager, nameof(manager));

                var processing = new Processing();

                foreach (var attribute in processingNode.Attributes.Cast<XmlAttribute>())
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

                var facilityNode = processingNode.SelectSingleNodeThrowIfNull("safe:facility", manager);
                if (facilityNode != null)
                    processing.Facility = ParseFacility(facilityNode, manager);

                var resourceNodes = processingNode.SelectNodes("safe:resource", manager);
                foreach (var currentResourceNode in resourceNodes.Cast<XmlNode>())
                {
                    processing.Resources.Add(ParseResource(currentResourceNode, manager));
                }

                return processing;
            }

            private static ProcessingResource ParseResource([NotNull]XmlNode resourceNode, [NotNull]XmlNamespaceManager manager)
            {
                Guard.NotNull(resourceNode, nameof(resourceNode));
                Guard.NotNull(manager, nameof(manager));

                var resource = new ProcessingResource();

                foreach (var attribute in resourceNode.Attributes.Cast<XmlAttribute>())
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

                var processingNodes = resourceNode.SelectNodes("safe:processing", manager);

                foreach (var currentProcessingNode in processingNodes.Cast<XmlNode>())
                {
                    resource.Processing = ParseProcessing(currentProcessingNode, manager);
                }

                return resource;
            }

            private static ProcessingFacility ParseFacility([NotNull]XmlNode facilityNode, [NotNull]XmlNamespaceManager manager)
            {
                Guard.NotNull(facilityNode, nameof(facilityNode));
                Guard.NotNull(manager, nameof(manager));

                var facility = new ProcessingFacility();

                foreach (var attribute in facilityNode.Attributes.Cast<XmlAttribute>())
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

                var softwareNodes = facilityNode.SelectNodes("safe:software", manager);

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

        private static class PlatformParser
        {
            public static Platform Parse([NotNull]XmlNode platformNode, [NotNull]XmlNamespaceManager manager)
            {
                Guard.NotNull(platformNode, nameof(platformNode));
                Guard.NotNull(manager, nameof(manager));

                var platform = new Platform { Instrument = new PlatformInstrument(), LeapSecondInformation = new LeapSecondInformation() };

                var dataNode = platformNode.SelectSingleNodeThrowIfNull("metadataWrap/xmlData/safe:platform", manager);

                var nssdcIdentifierNode = dataNode.SelectSingleNodeThrowIfNull("safe:nssdcIdentifier", manager);
                platform.NssdcIdentifier = nssdcIdentifierNode.InnerText;

                var familyNameNode = dataNode.SelectSingleNodeThrowIfNull("safe:familyName", manager);
                platform.FamilyName = familyNameNode.InnerText;

                var numberNode = dataNode.SelectSingleNodeThrowIfNull("safe:number", manager);
                platform.Number = numberNode.InnerText;

                var instrumentNode = dataNode.SelectSingleNodeThrowIfNull("safe:instrument", manager);

                var instrumentFamilyNameNode = instrumentNode.SelectSingleNodeThrowIfNull("safe:familyName", manager);
                platform.Instrument.Name = instrumentFamilyNameNode.InnerText;
                platform.Instrument.Abbreviation = instrumentFamilyNameNode.Attributes[0].InnerText;

                var instrumentModeNode = instrumentNode.SelectSingleNodeThrowIfNull("safe:extension/s1sarl1:instrumentMode/s1sarl1:mode", manager);
                platform.Instrument.Mode = (InstrumentModeType)Enum.Parse(typeof(InstrumentModeType), instrumentModeNode.InnerText);

                var swathNodes = instrumentNode.SelectNodes("safe:extension/s1sarl1:instrumentMode/s1sarl1:swath", manager);

                foreach (var currentSwathNode in swathNodes.Cast<XmlNode>())
                {
                    platform.Instrument.Swaths.Add((SwathType)Enum.Parse(typeof(SwathType), currentSwathNode.InnerText));
                }

                var extensionNode = dataNode.SelectSingleNodeThrowIfNull("safe:extension", manager);

                var leapSecondInformationNode = extensionNode?.SelectSingleNodeThrowIfNull("s1:leapSecondInformation", manager);

                if (leapSecondInformationNode != null)
                {
                    var utcTimeOfOccurrenceNode = leapSecondInformationNode.SelectSingleNodeThrowIfNull("s1:utcTimeOfOccurrence", manager);
                    platform.LeapSecondInformation.UtcTimeOfOccurence = DateTimeOffset.Parse(utcTimeOfOccurrenceNode.InnerText);

                    var signNode = leapSecondInformationNode.SelectSingleNodeThrowIfNull("s1:sign", manager);

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

        private static class MeasurementOrbitReferenceParser
        {
            public static MeasurementOrbitReference Parse([NotNull]XmlNode measurementOrbitReferenceNode, [NotNull]XmlNamespaceManager manager)
            {
                Guard.NotNull(measurementOrbitReferenceNode, nameof(measurementOrbitReferenceNode));
                Guard.NotNull(manager, nameof(manager));

                var measurementOrbitReference = new MeasurementOrbitReference();

                var dataNode = measurementOrbitReferenceNode.SelectSingleNodeThrowIfNull("metadataWrap/xmlData/safe:orbitReference", manager);

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

                var cycleNumberNode = dataNode.SelectSingleNodeThrowIfNull("safe:cycleNumber", manager);
                measurementOrbitReference.CycleNumber = int.Parse(cycleNumberNode.InnerText);

                var phaseIdentifierNode = dataNode.SelectSingleNodeThrowIfNull("safe:phaseIdentifier", manager);
                measurementOrbitReference.PhaseIdentifier = int.Parse(phaseIdentifierNode.InnerText);

                var passNode = dataNode.SelectSingleNodeThrowIfNull("safe:extension/s1:orbitProperties/s1:pass", manager);
                measurementOrbitReference.Pass = (PassType)Enum.Parse(typeof(PassType), passNode.InnerText);

                var ascendingNodeTimeNode = dataNode.SelectSingleNodeThrowIfNull("safe:extension/s1:orbitProperties/s1:ascendingNodeTime", manager);

                if (ascendingNodeTimeNode != null)
                    measurementOrbitReference.AscendingNodeTime = DateTimeOffset.Parse(ascendingNodeTimeNode.InnerText);

                return measurementOrbitReference;
            }
        }

        private static class MeasurementFrameParser
        {
            public static MeasurementFrameSet Parse([NotNull]XmlNode measurementFrameSetNode, [NotNull]XmlNamespaceManager manager)
            {
                Guard.NotNull(measurementFrameSetNode, nameof(measurementFrameSetNode));
                Guard.NotNull(manager, nameof(manager));

                var measureementFrameSet = new MeasurementFrameSet();

                var dataNodes = measurementFrameSetNode.SelectNodes("metadataWrap/xmlData/safe:frameSet/safe:frame", manager);

                foreach (var currentFrameNode in dataNodes.Cast<XmlNode>())
                {
                    measureementFrameSet.MeasurementFrames.Add(ParseFrame(currentFrameNode, manager));
                }

                return measureementFrameSet;
            }

            private static MeasurementFrame ParseFrame([NotNull]XmlNode measurementFrameNode, [NotNull]XmlNamespaceManager manager)
            {
                Guard.NotNull(measurementFrameNode, nameof(measurementFrameNode));
                Guard.NotNull(manager, nameof(manager));
                
                var measurementFrame = new MeasurementFrame();

                var numberNode = measurementFrameNode.SelectSingleNodeThrowIfNull("safe:number", manager);

                if (numberNode != null)
                    measurementFrame.Number = int.Parse(numberNode.InnerText);

                var footPrintNode = measurementFrameNode.SelectSingleNodeThrowIfNull("safe:footPrint", manager);
                measurementFrame.Footprint = footPrintNode.InnerText;

                return measurementFrame;
            }
        }

        private static class AcquisitionPeriodParser
        {
            public static AcquisitionPeriod Parse([NotNull]XmlNode acquisitionPeriodNode, [NotNull]XmlNamespaceManager manager)
            {
                Guard.NotNull(acquisitionPeriodNode, nameof(acquisitionPeriodNode));
                Guard.NotNull(manager, nameof(manager));

                var acquisitionPerid = new AcquisitionPeriod();

                var dataNode = acquisitionPeriodNode.SelectSingleNodeThrowIfNull("metadataWrap/xmlData/safe:acquisitionPeriod", manager);

                var startTimeNode = dataNode.SelectSingleNodeThrowIfNull("safe:startTime", manager);
                acquisitionPerid.StartTime = DateTimeOffset.Parse(startTimeNode.InnerText);
                var stopTimeNode = dataNode.SelectSingleNodeThrowIfNull("safe:stopTime", manager);
                acquisitionPerid.StopTime = DateTimeOffset.Parse(stopTimeNode.InnerText);

                var timeAnxNode = dataNode.SelectSingleNodeThrowIfNull("safe:extension/s1:timeANX", manager);

                var startTimeANXNode = timeAnxNode.SelectSingleNodeThrowIfNull("s1:startTimeANX", manager);
                acquisitionPerid.StartTimeANX = float.Parse(startTimeANXNode.InnerText);
                var stopTimeANXNode = timeAnxNode.SelectSingleNodeThrowIfNull("s1:stopTimeANX", manager);
                acquisitionPerid.StopTimeANX = float.Parse(stopTimeANXNode.InnerText);

                return acquisitionPerid;
            }
        }

        private static class GeneralProductInformationParser
        {
            public static GeneralProductInformation Parse([NotNull]XmlNode generalProductInformationNode, [NotNull] XmlNamespaceManager manager)
            {
                Guard.NotNull(generalProductInformationNode, nameof(generalProductInformationNode));
                Guard.NotNull(manager, nameof(manager));

                var generalProductInformation = new GeneralProductInformation();

                var dataNode = generalProductInformationNode.SelectSingleNodeThrowIfNull("metadataWrap/xmlData/s1sarl1:standAloneProductInformation", manager);

                var instrumentConfigurationIDNode = dataNode.SelectSingleNodeThrowIfNull("s1sarl1:instrumentConfigurationID", manager);
                generalProductInformation.InstrumentConfigurationID = int.Parse(instrumentConfigurationIDNode.InnerText);

                var missionDataTakeIDNode = dataNode.SelectSingleNodeThrowIfNull("s1sarl1:missionDataTakeID", manager);
                generalProductInformation.MissionDataTakeID = int.Parse(missionDataTakeIDNode.InnerText);

                var transmitterReceiverPolarisationNodes = dataNode.SelectNodes("s1sarl1:transmitterReceiverPolarisation", manager);
                foreach (var transmitterReceiverPolarisationNode in transmitterReceiverPolarisationNodes.Cast<XmlNode>())
                {
                    generalProductInformation.TransmitterReceiverPolarisation.Add((TransmitterReceiverPolarisationType)Enum.Parse(typeof(TransmitterReceiverPolarisationType), transmitterReceiverPolarisationNode.InnerText));
                }

                var productClassNode = dataNode.SelectSingleNodeThrowIfNull("s1sarl1:productClass", manager);
                generalProductInformation.ProductClass = (ProductClassType)Enum.Parse(typeof(ProductClassType), productClassNode.InnerText);

                var productClassDescriptionNode = dataNode.SelectSingleNodeThrowIfNull("s1sarl1:productClassDescription", manager);
                generalProductInformation.ProductClassDescription = (ProductClassDescriptionType)Enum.Parse(typeof(ProductClassDescriptionType), productClassDescriptionNode.InnerText.RemoveWhitespaces());

                var productCompositionNode = dataNode.SelectSingleNodeThrowIfNull("s1sarl1:productComposition", manager);
                generalProductInformation.ProductComposition = (ProductCompositionType)Enum.Parse(typeof(ProductCompositionType), productCompositionNode.InnerText);

                var productTypeNode = dataNode.SelectSingleNodeThrowIfNull("s1sarl1:productType", manager);
                generalProductInformation.ProductType = (ProductTypeType)Enum.Parse(typeof(ProductTypeType), productTypeNode.InnerText);

                var productTimelinessCategoryNode = dataNode.SelectSingleNodeThrowIfNull("s1sarl1:productTimelinessCategory", manager);
                generalProductInformation.ProductTimelinessCategory = (ProductTimelinessCategoryType)Enum.Parse(typeof(ProductTimelinessCategoryType), productTimelinessCategoryNode.InnerText.Replace("-", ""));

                var sliceProductFlagNode = dataNode.SelectSingleNodeThrowIfNull("s1sarl1:sliceProductFlag", manager);
                generalProductInformation.SlideProductFlag = bool.Parse(sliceProductFlagNode.InnerText);

                if (generalProductInformation.SlideProductFlag)
                {
                    var segmentStartTimeNode = dataNode.SelectSingleNodeThrowIfNull("s1sarl1:segmentStartTime", manager);
                    generalProductInformation.SegementStartTime = DateTimeOffset.Parse(segmentStartTimeNode.InnerText);

                    var sliceNumberNode = dataNode.SelectSingleNodeThrowIfNull("s1sarl1:sliceNumber", manager);
                    generalProductInformation.SliceNumber = int.Parse(sliceNumberNode.InnerText);

                    var totalSlicesNode = dataNode.SelectSingleNodeThrowIfNull("s1sarl1:totalSlices", manager);
                    generalProductInformation.TotalSlices = int.Parse(totalSlicesNode.InnerText);
                }

                return generalProductInformation;
            }
        }
    }
}