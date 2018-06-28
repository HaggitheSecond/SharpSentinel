using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using JetBrains.Annotations;
using SharpSentinel.Parser.Data.S1;
using SharpSentinel.Parser.Extensions;
using SharpSentinel.Parser.Helpers;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute

namespace SharpSentinel.Parser.Parsers
{
    public static class MeasurementDataUnitParser
    {
        public static IList<MeasurementDataUnit> Parse([NotNull]XmlNode informationPackageMap, [NotNull]XmlNode metaDataSection, [NotNull]XmlNode dataObjectSection, [NotNull]XmlNamespaceManager manager, [NotNull]DirectoryInfo baseDirectory)
        {
            Guard.NotNull(informationPackageMap, nameof(informationPackageMap));
            Guard.NotNull(metaDataSection, nameof(metaDataSection));
            Guard.NotNull(dataObjectSection, nameof(dataObjectSection));
            Guard.NotNull(manager, nameof(manager));
            Guard.NotNullAndValidFileSystemInfo(baseDirectory, nameof(baseDirectory));

            var measurementDataUnits = new List<MeasurementDataUnit>();
            var measurementDataUnitNodes = informationPackageMap.SelectNodes("xfdu:contentUnit/xfdu:contentUnit[@unitType='Measurement Data Unit']", manager);

            foreach (var currentMeasurementDataUnitNode in measurementDataUnitNodes.Cast<XmlNode>())
            {
                measurementDataUnits.Add(ParseMeasurementUnit(currentMeasurementDataUnitNode, metaDataSection, dataObjectSection, manager, baseDirectory));
            }

            return measurementDataUnits;
        }

        private static MeasurementDataUnit ParseMeasurementUnit([NotNull]XmlNode informationPackageMapNode, [NotNull]XmlNode metaDataSection, [NotNull]XmlNode dataObjectNode, [NotNull]XmlNamespaceManager manager, [NotNull]DirectoryInfo baseDirectory)
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
                    var annotationRepId = annotationDataObject.GetAttributeValue("repID123");

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