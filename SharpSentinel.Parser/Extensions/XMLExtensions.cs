using System;
using System.IO;
using System.Xml;
using JetBrains.Annotations;
using SharpSentinel.Parser.Data.Common;
using SharpSentinel.Parser.Exceptions.XML;
using SharpSentinel.Parser.Helpers;

namespace SharpSentinel.Parser.Extensions
{
    public static class XMLExtensions
    {
        public static XmlNode SelectMetaDataObjectByID(this XmlNode self, string id)
        {
            return self.SelectSingleNode($"metadataObject[@ID='{id}']");
        }

        public static XmlNode SelectedDataObjectById(this XmlNode self, string id)
        {
            return self.SelectSingleNode($"dataObject[@ID='{id}']");
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
                .SelectSingleNode("byteStream/fileLocation");

            if (fileLocationNode == null)
                throw new XmlNodeNotFoundException();

            var fileLocationRawAttributes = fileLocationNode
                .Attributes;

            if (fileLocationRawAttributes == null)
                throw new XmlException();

            var fileLocation = fileLocationRawAttributes
                .GetNamedItem("href")
                .Value;

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
                .SelectSingleNode("byteStream/checksum");

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