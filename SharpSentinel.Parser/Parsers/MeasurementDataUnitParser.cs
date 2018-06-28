using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using SharpSentinel.Parser.Data.S1;
using SharpSentinel.Parser.Extensions;
// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute

namespace SharpSentinel.Parser.Parsers
{
    public static class MeasurementDataUnitParser
    {
        public static IList<MeasurementDataUnit> Parse(XmlNode informationPackageMap, XmlNode dataObjectSection, XmlNode metaDataSection, XmlNamespaceManager manager, DirectoryInfo baseDirectory)
        {
            var measurementDataUnits = new List<MeasurementDataUnit>();
            var measurementDataUnitNodes = informationPackageMap.SelectNodes("xfdu:contentUnit/xfdu:contentUnit[@unitType='Measurement Data Unit']", manager);

            foreach (var currentMeasurementDataUnitNode in measurementDataUnitNodes.Cast<XmlNode>())
            {
                var dataObjectId = currentMeasurementDataUnitNode
                    .SelectSingleNode("dataObjectPointer", manager)
                    .Attributes
                    .GetNamedItem("dataObjectID")
                    .Value;

                var dataObjectNode = dataObjectSection.SelectedDataObjectById(dataObjectId);

                measurementDataUnits.Add(ParseMeasurementUnit(currentMeasurementDataUnitNode, dataObjectNode, metaDataSection, manager, baseDirectory));
            }

            return measurementDataUnits;
        }

        private static MeasurementDataUnit ParseMeasurementUnit(XmlNode informationPackageMapNode, XmlNode dataObjectNode, XmlNode metaDataSection, XmlNamespaceManager manager, DirectoryInfo baseDirectory)
        {
            var measurementDataUnit = new MeasurementDataUnit();

            var repId = informationPackageMapNode
                .Attributes
                .GetNamedItem("repID")
                .Value;

            if (repId == "s1Level1MeasurementSchema")
            {
                measurementDataUnit.MeasurementDataUnitType = MeasurementDataUnitType.Measurement;

                var dmdIds = informationPackageMapNode
                    .Attributes
                    .GetNamedItem("dmdID")
                    .Value
                    .Split(' ')
                    .Where(f => string.IsNullOrWhiteSpace(f) == false);

                foreach (var currentDmdId in dmdIds)
                {
                    var annotationMetaDataNode = metaDataSection.SelectMetaDataObjectByID(currentDmdId);
                    var annotationDataObjectId = annotationMetaDataNode
                        .SelectSingleNode("dataObjectPointer")
                        .Attributes
                        .GetNamedItem("dataObjectID")
                        .Value;

                    var annotationDataObject = dataObjectNode.SelectedDataObjectById(annotationDataObjectId);

                    var annotationFileLocation = annotationDataObject.GetFileInfoFromDataObject(baseDirectory);
                    var annotationChecksum = annotationDataObject.GetChecksumFromDataObject();
                    var annotationRepId = annotationDataObject.GetAttributeValue("repID");

                    switch (annotationRepId)
                    {
                        case "s1Level1ProductSchema":
                            measurementDataUnit.ProductAnnotation = AnnotationParser.ParseProductAnnotation(annotationFileLocation, annotationChecksum);
                            break;

                        case "s1Level1NoiseSchema":
                            measurementDataUnit.NoiseAnnotation = AnnotationParser.ParseNoiseAnnotation(annotationFileLocation, annotationChecksum);
                            break;

                        case "s1Level1CalibrationSchema":
                            measurementDataUnit.CalibriationAnnotation = AnnotationParser.ParseCalibriationAnnotation(annotationFileLocation, annotationChecksum);
                            break;

                        default:
                            throw new XmlException($"Unknown repID: {annotationRepId}");
                    }
                }
            }
            else if (repId == "s1Level1QuickLookSchema")
            {
                measurementDataUnit.MeasurementDataUnitType = MeasurementDataUnitType.QuickLook;
            }
            else
            {
                throw new XmlException("Found unknown measurement data unit");
            }

            measurementDataUnit.File = dataObjectNode.GetFileInfoFromDataObject(baseDirectory);
            measurementDataUnit.Checksum = dataObjectNode.GetChecksumFromDataObject();

            return measurementDataUnit;
        }
    }
}