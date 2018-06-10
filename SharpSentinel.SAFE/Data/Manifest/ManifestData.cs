using System.Xml;
using SharpSentinel.SAFE.Data.Internal.FileTypes;

namespace SharpSentinel.SAFE.Data
{
    public sealed class ManifestData : XMLFile
    {
        public ManifestData(string filePath, XmlDocument document)
        : base(filePath, document)
        {

        }
    }
}