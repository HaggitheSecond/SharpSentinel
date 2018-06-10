using System.Xml;
using SharpSentinel.Parser.Data.Internal.FileTypes;

namespace SharpSentinel.Parser.Data.Manifest
{
    public sealed class ManifestData : XMLFile
    {
        public ManifestData(string filePath, XmlDocument document)
        : base(filePath, document)
        {

        }
    }
}