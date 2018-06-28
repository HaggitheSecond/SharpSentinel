using System.IO;

namespace SharpSentinel.Parser.Data.ManifestObjects
{
    public class Manifest
    {
        /// <summary>
        /// Metadata saved directly in the manifest
        /// </summary>
        public MetaData MetaData { get; set; }

        /// <summary>
        /// Raw XML of manifest.safe
        /// </summary>
        public string RawXML { get; set; }

        /// <summary>
        /// Systemdata for the manifest.safe file
        /// </summary>
        public FileInfo File { get; set; }
    }
}