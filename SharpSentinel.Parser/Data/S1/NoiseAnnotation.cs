using System.IO;
using SharpSentinel.Parser.Data.Common;

namespace SharpSentinel.Parser.Data.S1
{
    public class NoiseAnnotation
    {
        public FileInfo File { get; set; }

        public string RawXML { get; set; }

        public Checksum Checksum { get; set; }
    }
}