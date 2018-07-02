using System.IO;
using SharpSentinel.Parser.Data.Interfaces;

namespace SharpSentinel.Parser.Data.ManifestObjects
{
    public class Manifest : IFile, IXmlFile
    {
        /// <summary>
        /// Metadata saved directly in the manifest
        /// </summary>
        public MetaData MetaData { get; set; }

        /// <summary>
        /// Raw XML of manifest.safe
        /// </summary>
        public string RawXml { get; set; }

        /// <summary>
        /// Systemdata for the manifest.safe file
        /// </summary>
        public FileInfo File { get; set; }
    }
}