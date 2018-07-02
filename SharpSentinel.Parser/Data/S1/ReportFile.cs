using System.IO;
using SharpSentinel.Parser.Data.Interfaces;

namespace SharpSentinel.Parser.Data.S1
{
    public class ReportFile : IFile
    {
        /// <summary>
        /// Systemdata for the .png file
        /// </summary>
        public FileInfo File { get; set; }
    }
}