using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpSentinel.Parser.Data.Internal;
using SharpSentinel.Parser.Extensions;

namespace SharpSentinel.Parser.Helpers
{
    internal static class FileHelper
    {
        /// <summary>
        /// Loads the specified files
        /// </summary>
        /// <param name="directoryPath">Path to the SAFE directory</param>
        /// <param name="fileType">The desired filetype</param>
        /// <returns>All files that match the filetype</returns>
        public static IEnumerable<FileInfo> GetFiles(string directoryPath, SAFEFileTypes fileType)
        {
            if (Directory.Exists(directoryPath) == false)
                throw new DirectoryNotFoundException();

            return new DirectoryInfo(directoryPath).GetFiles().ToList().Where(f => f.Name.Contains(fileType.GetFileName()));
        }
    }
}