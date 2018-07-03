using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using JetBrains.Annotations;
using SharpSentinel.Parser.Data.Common;
using SharpSentinel.Parser.Exceptions.XML;
using SharpSentinel.Parser.Helpers;

namespace SharpSentinel.Parser.Extensions
{
    public static class XMLExtensions
    {
        public static XmlNode SelectSingleNodeThrowIfNull(this XmlNode self, string xPath, XmlNamespaceManager namespaceManager = null)
        {
            Guard.NotNull(self, nameof(self));

            var foundNode = self.SelectSingleNode(xPath, namespaceManager);

            if(foundNode == null)
                throw new XmlException($"Could not find node: {xPath} in {self.Name}");

            return foundNode;
        }

        public static XmlNode SelectMetaDataObjectByID(this XmlNode self, string id)
        {
            return self.SelectSingleNodeThrowIfNull($"metadataObject[@ID='{id}']");
        }

        public static XmlNode SelectedDataObjectById(this XmlNode self, string id)
        {
            return self.SelectSingleNodeThrowIfNull($"dataObject[@ID='{id}']");
        }

        public static string GetAttributeValue(this XmlNode self, string attributeName)
        {
            var attributes = self.Attributes;

            if(attributes == null)
                throw new XmlException($"No attributes found for {self.Name}");

            var attribute = attributes.GetNamedItem(attributeName);

            if(attribute == null)
                throw new XmlException($"Attribute with id {attributeName} not found for {self.Name}");

            return attribute.Value;
        }

        public static FileInfo GetFileInfoFromDataObject(this XmlNode dataObjectNode, [NotNull] DirectoryInfo baseDirectory)
        {
            Guard.NotNull(dataObjectNode, nameof(dataObjectNode));
            Guard.NotNullAndValidFileSystemInfo(baseDirectory, nameof(baseDirectory));

            var fileLocationNode = dataObjectNode
                .SelectSingleNodeThrowIfNull("byteStream/fileLocation");

            var fileLocationRawAttributes = fileLocationNode
                .Attributes;

            if (fileLocationRawAttributes == null)
                throw new XmlException();

            var fileLocation = fileLocationRawAttributes
                .GetNamedItem("href");

            return fileLocation.GetFileInfoFromHrefAttribute(baseDirectory);
        }

        public static FileInfo GetFileInfoFromHrefAttribute(this XmlNode hrefAttribute, [NotNull] DirectoryInfo baseDirectory)
        {
            var fileLocation = hrefAttribute.Value;

            if (fileLocation.StartsWith("./"))
                fileLocation = fileLocation.Remove(0, 2);

            fileLocation = fileLocation.Replace(@"/", @"\");

            var filePath = Path.Combine(baseDirectory.FullName, fileLocation);

            if (File.Exists(filePath) == false)
                throw new FileNotFoundException(filePath);

            return new FileInfo(filePath);
        }

        public static Checksum GetChecksumFromDataObject(this XmlNode dataObjectNode)
        {
            Guard.NotNull(dataObjectNode, nameof(dataObjectNode));

            var checksum = new Checksum();

            var checksumNode = dataObjectNode
                .SelectSingleNodeThrowIfNull("byteStream/checksum");

            if (checksumNode == null)
                throw new XmlException();

            var checksumAttributes = checksumNode
                .Attributes;

            if (checksumAttributes == null)
                throw new XmlException();

            checksum.Name = checksumAttributes
                .GetNamedItem("checksumName")
                .Value;

            checksum.Sum = checksumNode.InnerText;

            return checksum;
        }
    }
}