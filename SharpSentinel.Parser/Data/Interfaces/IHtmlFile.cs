namespace SharpSentinel.Parser.Data.Interfaces
{
    public interface IHtmlFile : IFile
    {
        string HtmlText { get; set; }
    }
}