using System;
using SharpSentinel.SAFE.Data.Internal;
using SharpSentinel.SAFE.Helpers;

namespace SharpSentinel.SAFE.Extensions
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