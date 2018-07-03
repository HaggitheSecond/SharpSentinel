using System.IO;
using SharpSentinel.Parser.Data.Interfaces;

namespace SharpSentinel.Parser.Data.Common
{
    public class Documentation : IXmlFile
    {
        /// <summary>
        /// Systemdata for the .xml file
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// The raw XML from the file
        /// </summary>
        public string RawXml { get; set; }
    }
}