using System.Xml;

namespace SharpSentinel.SAFE.Data
{
    public class ManifestData
    {
        public XmlDocument Document { get; }

        public ManifestData(XmlDocument document)
        {
            this.Document = document;
        }
    }
}