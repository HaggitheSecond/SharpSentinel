using System.IO;
using System.Xml;
using JetBrains.Annotations;
using SharpSentinel.Parser.Data.S1;
using SharpSentinel.Parser.Data.S1.Preview;
using SharpSentinel.Parser.Extensions;
using SharpSentinel.Parser.Helpers;
// ReSharper disable MemberHidesStaticFromOuterClass

namespace SharpSentinel.Parser.Parsers
{
    public static class PreviewParser
    {
        public static Preview Parse([NotNull] XmlNode informationPackageMap,
            [NotNull] XmlNode dataObjectSection,
            [NotNull] XmlNamespaceManager manager,
            [NotNull] DirectoryInfo baseDirectory)
        {
            Guard.NotNull(informationPackageMap, nameof(informationPackageMap));
            Guard.NotNull(dataObjectSection, nameof(dataObjectSection));
            Guard.NotNull(manager, nameof(manager));
            Guard.NotNullAndValidFileSystemInfo(baseDirectory, nameof(baseDirectory));

            var preview = new Preview();

            preview.QuickLook = QuickLookParser.Parse(informationPackageMap, dataObjectSection, manager, baseDirectory);
            preview.MapOverlay = MapOverlayParser.Parse(informationPackageMap, dataObjectSection, manager, baseDirectory);
            preview.ProductPreview = ProductPreviewParser.Parse(informationPackageMap, dataObjectSection, manager, baseDirectory);

            return preview;
        }

        public static class QuickLookParser
        {
            public static QuickLookDataUnit Parse([NotNull] XmlNode informationPackageMap,
                [NotNull] XmlNode dataObjectSection,
                [NotNull] XmlNamespaceManager manager,
                [NotNull] DirectoryInfo baseDirectory)
            {
                var quickLook = new QuickLookDataUnit();

                var quickLookNode = informationPackageMap
                    .SelectSingleNodeThrowIfNull("xfdu:contentUnit/xfdu:contentUnit[@repID='s1Level1QuickLookSchema']", manager);
                var quickLookObjectNode = dataObjectSection
                    .SelectedDataObjectById(quickLookNode.SelectSingleNode("dataObjectPointer").GetAttributeValue("dataObjectID"));

                quickLook.File = quickLookObjectNode.GetFileInfoFromDataObject(baseDirectory);
                quickLook.Checksum = quickLookObjectNode.GetChecksumFromDataObject();

                return quickLook;
            }
        }

        public static class MapOverlayParser
        {
            public static MapOverlay Parse([NotNull] XmlNode informationPackageMap,
                [NotNull] XmlNode dataObjectSection,
                [NotNull] XmlNamespaceManager manager,
                [NotNull] DirectoryInfo baseDirectory)
            {
                var mapOverlay = new MapOverlay();

                var mapOverlayNode = informationPackageMap
                    .SelectSingleNodeThrowIfNull("xfdu:contentUnit/xfdu:contentUnit[@repID='s1Level1MapOverlaySchema']", manager);

                var mapOverlayObjectNode = dataObjectSection
                    .SelectedDataObjectById(mapOverlayNode.SelectSingleNode("dataObjectPointer").GetAttributeValue("dataObjectID"));

                mapOverlay.File = mapOverlayObjectNode.GetFileInfoFromDataObject(baseDirectory);

                return mapOverlay;
            }
        }

        public static class ProductPreviewParser
        {
            public static ProductPreview Parse([NotNull] XmlNode informationPackageMap,
                [NotNull] XmlNode dataObjectSection,
                [NotNull] XmlNamespaceManager manager,
                [NotNull] DirectoryInfo baseDirectory)
            {
                var productPreview = new ProductPreview();

                var productPreviewNode = informationPackageMap
                    .SelectSingleNodeThrowIfNull("xfdu:contentUnit/xfdu:contentUnit[@repID='s1Level1ProductPreviewSchema']", manager);
                var productPreviewObjectNode = dataObjectSection
                    .SelectedDataObjectById(productPreviewNode.SelectSingleNode("dataObjectPointer").GetAttributeValue("dataObjectID"));

                productPreview.File = productPreviewObjectNode.GetFileInfoFromDataObject(baseDirectory);
                productPreview.HtmlText = File.ReadAllText(productPreview.File.FullName);

                return productPreview;
            }
        }
    }
}