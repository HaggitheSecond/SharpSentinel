using System.IO;
using System.Linq;
using System.Xml;
using SharpSentinel.Parser.Data.Internal;
using SharpSentinel.Parser.Data.Manifest;
using SharpSentinel.Parser.Helpers;

namespace SharpSentinel.Parser.Parsers
{
    internal static class ManifestParser
    {
        /// <summary>
        /// Parses the manifest.safe data
        /// </summary>
        /// <param name="directory">Path to the SAFE directory</param>
        /// <returns>The parsed manifest data</returns>
        public static Manifest Parse(string directory)
        {
            var file = FileHelper.GetFiles(directory, SAFEFileTypes.Manifest).First();
            
            using (var fileStream = new FileStream(file.FullName, FileMode.Open))
            {
                var document = new XmlDocument();
                document.Load(fileStream);
              
                return new Manifest(file.FullName, document);
            }

        }
    }
}