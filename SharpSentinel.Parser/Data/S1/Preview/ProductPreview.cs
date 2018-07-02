using System.IO;
using SharpSentinel.Parser.Data.Interfaces;

namespace SharpSentinel.Parser.Data.S1.Preview
{
    public class ProductPreview : IHtmlFile
    {
        /// <summary>
        /// Systemdata for the .html file
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// The html text in the file
        /// </summary>
        public string HtmlText { get; set; }
    }
}