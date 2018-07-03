using System.IO;
using SharpSentinel.Parser.Data.Common;
using SharpSentinel.Parser.Data.Interfaces;

namespace SharpSentinel.Parser.Data.S1.Preview
{
    public class MapOverlay : IFile, IHaveDocumentation
    {
        /// <summary>
        /// Systemdata for the file
        /// </summary>
        public FileInfo File { get; set; }
        
        /// <summary>
        /// The .xsd documentation for this .kml file. For this file it does not contain any actual documentation.
        /// </summary>
        public Documentation Documentation { get; set; }
    }
}