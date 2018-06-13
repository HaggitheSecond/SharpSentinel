using System.IO;
using System.Linq;
using SharpSentinel.Parser.Data.Internal;
using SharpSentinel.Parser.Extensions;

namespace SharpSentinel.Parser.Helpers
{
    internal static class DirectoryHelper
    {
        /// <summary>
        /// Ensure the given directory is formated correctly for sentinel 1 sar-c data
        /// </summary>
        /// <param name="directoryPath">Path to the SAFE directory</param>
        public static void EnsureS1SAFEDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath) == false)
                throw new DirectoryNotFoundException();

            var directory = new DirectoryInfo(directoryPath);
            var files = directory.GetFiles().ToList();

            if (files.Any(f => f.Name == SAFEFileTypes.Manifest.GetFileName()) == false)
                throw new FileNotFoundException($"{SAFEFileTypes.Manifest.GetFileName()} not found");
        }
    }
}