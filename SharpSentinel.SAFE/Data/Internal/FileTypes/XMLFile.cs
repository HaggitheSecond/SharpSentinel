using System.Xml;

namespace SharpSentinel.Parser.Data.Internal.FileTypes
{
    public abstract class XMLFile : DataFile
    {
        public XmlDocument Document { get; }

        internal XMLFile(string filePath, XmlDocument document)
        : base(filePath)
        {
            this.Document = document;
            this.LoadDataFromXML();
        }

        public virtual void LoadDataFromXML()
        {

        }
    }
}