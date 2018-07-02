namespace SharpSentinel.Parser.Data.Interfaces
{
    public interface IXmlFile : IFile
    {
        string RawXml { get; set; }
    }
}