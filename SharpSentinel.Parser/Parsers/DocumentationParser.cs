using System.IO;
using System.Xml;
using JetBrains.Annotations;
using SharpSentinel.Parser.Data.Common;
using SharpSentinel.Parser.Extensions;

namespace SharpSentinel.Parser.Parsers
{
    public static class DocumentationParser
    {
        public static Documentation ParseMapOverlayDocumentation([NotNull] XmlNode metaDataSection,
            [NotNull] XmlNamespaceManager manager,
            [NotNull] DirectoryInfo baseDirectory)
        {
            return ParseDocumentation(metaDataSection, manager, baseDirectory, "s1Level1MapOverlaySchema");
        }

        public static Documentation ParseQuickLookDocumentation([NotNull] XmlNode metaDataSection,
            [NotNull] XmlNamespaceManager manager,
            [NotNull] DirectoryInfo baseDirectory)
        {
            return ParseDocumentation(metaDataSection, manager, baseDirectory, "s1Level1QuickLookSchema");
        }

        public static Documentation ParseProductPreviewDocumentation([NotNull] XmlNode metaDataSection,
            [NotNull] XmlNamespaceManager manager,
            [NotNull] DirectoryInfo baseDirectory)
        {
            return ParseDocumentation(metaDataSection, manager, baseDirectory, "s1Level1ProductPreviewSchema");
        }

        public static Documentation ParseMeasurementDataUnitDocumentation([NotNull] XmlNode metaDataSection,
            [NotNull] XmlNamespaceManager manager,
            [NotNull] DirectoryInfo baseDirectory)
        {
            return ParseDocumentation(metaDataSection, manager, baseDirectory, "s1Level1MeasurementSchema");
        }

        public static (Documentation noiseDocumentation, Documentation productDocumentation, Documentation calibrationDocumentation) ParseAnnotationDocumentations([NotNull] XmlNode metaDataSection,
            [NotNull] XmlNamespaceManager manager,
            [NotNull] DirectoryInfo baseDirectory)
        {

            var noiseDocumentation = ParseDocumentation(metaDataSection, manager, baseDirectory, "s1Level1NoiseSchema");
            var calibrationDocumentation = ParseDocumentation(metaDataSection, manager, baseDirectory, "s1Level1CalibrationSchema");
            var productDocumentation = ParseDocumentation(metaDataSection, manager, baseDirectory, "s1Level1ProductSchema");

            return (noiseDocumentation, productDocumentation, calibrationDocumentation);
        }

        private static Documentation ParseDocumentation([NotNull] XmlNode metaDataSection,
            [NotNull] XmlNamespaceManager manager,
            [NotNull] DirectoryInfo baseDirectory,
            [NotNull] string metaDataObjectID)
        {
            var documentation = new Documentation();

            var metaDataNode = metaDataSection.SelectMetaDataObjectByID(metaDataObjectID);

            var filePath = metaDataNode.SelectSingleNodeThrowIfNull("metadataReference")
                .Attributes
                .GetNamedItem("href")
                .GetFileInfoFromHrefAttribute(baseDirectory);

            documentation.File = filePath;

            using (var file = File.Open(documentation.File.FullName, FileMode.Open))
            {
                var document = new XmlDocument();
                document.Load(file);

                documentation.RawXml = document.InnerXml;
            }

            return documentation;
        }
    }
}