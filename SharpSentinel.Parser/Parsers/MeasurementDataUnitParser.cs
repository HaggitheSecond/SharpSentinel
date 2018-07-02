using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using JetBrains.Annotations;
using SharpSentinel.Parser.Data.S1;
using SharpSentinel.Parser.Extensions;
using SharpSentinel.Parser.Helpers;
using SharpSentinel.Parser.Parsers.Annotations;

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

            var measurementDataUnitNodes = informationPackageMap.SelectNodes("xfdu:contentUnit/xfdu:contentUnit[@repID='s1Level1MeasurementSchema']", manager);

            return measurementDataUnitNodes.Cast<XmlNode>().Select(f => ParseMeasurementUnit(f, metaDataSection, dataObjectSection, manager, baseDirectory)).ToList();
        }

        private static MeasurementDataUnit ParseMeasurementUnit([NotNull]XmlNode informationPackageMapNode, [NotNull]XmlNode metaDataSection, [NotNull]XmlNode dataObjectSection, [NotNull]XmlNamespaceManager manager, [NotNull]DirectoryInfo baseDirectory)
        {
            var measurementDataUnit = new MeasurementDataUnit();

            var objectIdNode = informationPackageMapNode.SelectSingleNodeThrowIfNull("dataObjectPointer");
            var objectId = objectIdNode.GetAttributeValue("dataObjectID");
            var measurementDataObject = dataObjectSection.SelectedDataObjectById(objectId);
            
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
                    .SelectSingleNodeThrowIfNull("dataObjectPointer")
                    .Attributes
                    .GetNamedItem("dataObjectID")
                    .Value;

                var annotationDataObject = dataObjectSection.SelectedDataObjectById(annotationDataObjectId);

                var annotationFileLocation = annotationDataObject.GetFileInfoFromDataObject(baseDirectory);
                var annotationChecksum = annotationDataObject.GetChecksumFromDataObject();
                var annotationRepId = annotationDataObject.GetAttributeValue("repID");

                switch (annotationRepId)
                {
                    case "s1Level1ProductSchema":
                        measurementDataUnit.ProductAnnotation = ProductAnnotationParser.Parse(annotationFileLocation, annotationChecksum);
                        break;

                    case "s1Level1NoiseSchema":
                        measurementDataUnit.NoiseAnnotation = NoiseAnnotationParser.Parse(annotationFileLocation, annotationChecksum);
                        break;

                    case "s1Level1CalibrationSchema":
                        measurementDataUnit.CalibriationAnnotation = CalibrationAnnotationParser.Parse(annotationFileLocation, annotationChecksum);
                        break;

                    default:
                        throw new XmlException($"Unknown repID: {annotationRepId}");
                }
            }

            measurementDataUnit.File = measurementDataObject.GetFileInfoFromDataObject(baseDirectory);
            measurementDataUnit.Checksum = measurementDataObject.GetChecksumFromDataObject();

            return measurementDataUnit;
        }
    }
}