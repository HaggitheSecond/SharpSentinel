using System.IO;
using SharpSentinel.Parser.Data.Internal;
using SharpSentinel.Parser.Exceptions;

namespace SharpSentinel.Parser.Helpers
{
    internal static class SAFEHelper
    {
        /// <summary>
        /// Detects the platform (sentinel 1 or sentinel 2) by the foldername
        /// </summary>
        /// <param name="directoryPath">Path to the SAFE directory</param>
        /// <returns>The platform</returns>
        internal static Platform DetectPlatform(string directoryPath)
        {
            if (Directory.Exists(directoryPath) == false)
                throw new DirectoryNotFoundException();

            var directory = new DirectoryInfo(directoryPath);

            if (directory.Name.StartsWith("S1"))
                return Platform.Sentinel1;
            if (directory.Name.StartsWith("S2"))
                return Platform.Sentinel2;

            throw new SAFEDirectoryMalformedException();
        }
    }
}