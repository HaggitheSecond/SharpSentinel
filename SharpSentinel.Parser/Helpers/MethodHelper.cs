using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using SharpSentinel.Parser.Extensions;
using SharpSentinel.Parser.Parsers;

namespace SharpSentinel.Parser.Helpers
{
    public static class MethodHelper
    {
        public static bool TryGetPropertyDescription(Type type, string propertyName, out string propertyDescription)
        {
            propertyDescription = string.Empty;
            var xmlFile = GetXmlDescriptionFile();

            using (var file = File.Open(xmlFile.FullName, FileMode.Open))
            {
                var document = new XmlDocument();
                document.Load(file);

                var membersNode = document.SelectSingleNode("doc/members");
                if (membersNode == null)
                    throw new XmlException();

                var fullPropertyName = type.FullName + "." + propertyName;

                var memberNode = membersNode.SelectSingleNode($"member[contains(@name,'{fullPropertyName}')]");
                if (memberNode == null)
                    return false;

                var summaryNode = memberNode.SelectSingleNode("summary");
                if (summaryNode == null)
                    throw new XmlException();

                propertyDescription = summaryNode.InnerText.Trim();

                return true;
            }
        }

        public static bool TryGetPropertyDescriptions(Type type, out Dictionary<string, string> propertyDescriptions)
        {
            propertyDescriptions = new Dictionary<string, string>();
            var xmlFile = GetXmlDescriptionFile();

            using (var file = File.Open(xmlFile.FullName, FileMode.Open))
            {
                var document = new XmlDocument();
                document.Load(file);

                var membersNode = document.SelectSingleNode("doc/members");
                if (membersNode == null)
                    throw new XmlException();

                var memberNodes = membersNode.SelectNodes($"member[contains(@name,'{type.FullName}')]");
                if (memberNodes == null)
                    return false;

                foreach (var currentMemeberNode in memberNodes.Cast<XmlNode>())
                {
                    var summaryNode = currentMemeberNode.SelectSingleNode("summary");
                    if (summaryNode == null)
                        throw new XmlException();

                    var propertyName = currentMemeberNode.GetAttributeValue("name").Split('.').Last();

                    propertyDescriptions.Add(propertyName, summaryNode.InnerText);
                }

                return true;
            }
        }

        private static FileInfo GetXmlDescriptionFile()
        {
            var xmlFileName = Assembly.GetAssembly(typeof(MethodHelper)).FullName.Split(',').First() + ".xml";
            var xmlFile = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles().ToList().FirstOrDefault(f => f.Name == xmlFileName);

            if (xmlFile == null || File.Exists(xmlFile.FullName) == false)
                return null;

            return xmlFile;
        }
    }
}