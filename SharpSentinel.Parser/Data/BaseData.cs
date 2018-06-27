using System.IO;
using SharpSentinel.Parser.Data.ManifestObjects;

namespace SharpSentinel.Parser.Data
{
    public class BaseData
    {
        public Manifest Manifest { get; set; }

        public DirectoryInfo BaseDirectory { get; set; }
    }
}