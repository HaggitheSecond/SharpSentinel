using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using SharpSentinel.SAFE.Data;
using SharpSentinel.SAFE.Data.Internal;
using SharpSentinel.SAFE.Helpers;

namespace SharpSentinel.SAFE.Parsers
{
    internal static class ManifestParser
    {
        /// <summary>
        /// Parses the manifest.safe data
        /// </summary>
        /// <param name="directory">Path to the SAFE directory</param>
        /// <returns>The parsed manifest data</returns>
        public static ManifestData Parse(string directory)
        {
            var file = FileHelper.GetFiles(directory, SAFEFileTypes.Manifest).First();
            
            using (var fileStream = new FileStream(file.FullName, FileMode.Open))
            {
                var document = new XmlDocument();
                document.Load(fileStream);
                
                return new ManifestData(document);
            }

        }
    }
}