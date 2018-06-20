using System.Xml;

namespace SharpSentinel.Parser.Extensions
{
    public static class XMLExtensions
    {
        public static XmlNode SelectMetaDataObjectByID(this XmlNode self, string id)
        {
            return self.SelectSingleNode($"metadataObject[@ID='{id}']");
        }
    }
}