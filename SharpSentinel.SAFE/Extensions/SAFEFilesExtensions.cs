using System;
using SharpSentinel.Parser.Data.Internal;

namespace SharpSentinel.Parser.Extensions
{
    internal static class SAFEFilesExtensions
    {
        public static string GetFileName(this SAFEFileTypes fileType)
        {
            switch (fileType)
            {
                case SAFEFileTypes.Manifest:
                    return "manifest.safe";
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null);
            }
        }
    }
}