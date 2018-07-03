using System.IO;
using SharpSentinel.Parser.Data.Common;
using SharpSentinel.Parser.Data.Interfaces;

namespace SharpSentinel.Parser.Data.S1.Preview
{
    public class ProductPreview : IHtmlFile, IHaveDocumentation
    {
        /// <summary>
        /// Systemdata for the .html file
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// The html text in the file
        /// </summary>
        public string HtmlText { get; set; }
        
        /// <summary>
        /// The .xsd documentation for this .html file. For this file it does not contain any actual documentation.
        /// </summary>
        public Documentation Documentation { get; set; }
    }
}