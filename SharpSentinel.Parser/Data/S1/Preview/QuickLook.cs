using System.IO;
using SharpSentinel.Parser.Data.Common;
using SharpSentinel.Parser.Data.Interfaces;

namespace SharpSentinel.Parser.Data.S1.Preview
{
    public class QuickLook : IImageFile, IHaveDocumentation
    {
        /// <summary>
        /// Systemdata for the file
        /// </summary>
        public FileInfo File { get; set; }
        
        /// <summary>
        /// The checksum included in the manifest.safe
        /// </summary>
        public Checksum Checksum { get; set; }

        /// <summary>
        /// The .xsd documentation for this .png file. For this file it does not contain any actual documentation.
        /// </summary>
        public Documentation Documentation { get; set; }
    }
}