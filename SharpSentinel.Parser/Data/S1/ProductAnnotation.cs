using System.IO;
using SharpSentinel.Parser.Data.Common;
using SharpSentinel.Parser.Data.S1.Annotations;

namespace SharpSentinel.Parser.Data.S1
{
    public class ProductAnnotation
    {
        public FileInfo File { get; set; }

        public string RawXML { get; set; }

        public Checksum Checksum { get; set; }

        public AdsHeader AdsHeader { get; set; }

        public QualityInformation QualityInformation { get; set; }
    }
}