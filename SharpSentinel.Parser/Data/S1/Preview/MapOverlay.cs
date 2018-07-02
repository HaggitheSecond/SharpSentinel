using System.IO;
using SharpSentinel.Parser.Data.Interfaces;

namespace SharpSentinel.Parser.Data.S1.Preview
{
    public class MapOverlay : IFile
    {
        /// <summary>
        /// Systemdata for the file
        /// </summary>
        public FileInfo File { get; set; }
    }
}