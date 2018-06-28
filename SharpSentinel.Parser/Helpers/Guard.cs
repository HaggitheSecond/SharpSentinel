using System;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;

namespace SharpSentinel.Parser.Helpers
{
    public static class Guard
    {
        [DebuggerStepThrough]
        public static void NotNull(object argument, string argumentName)
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName);
        }

        [DebuggerStepThrough]
        public static void NotNullAndValidFileSystemInfo(FileSystemInfo fileSystemInfo, string argumentName)
        {
            if (fileSystemInfo == null)
                throw new ArgumentNullException(argumentName);

            if (fileSystemInfo is DirectoryInfo)
                if(Directory.Exists(fileSystemInfo.FullName) == false)
                    throw new DirectoryNotFoundException($"argumentName: {argumentName} directoryName: {fileSystemInfo.FullName}");

            if(fileSystemInfo is FileInfo)
                if(File.Exists(fileSystemInfo.FullName) == false)
                    throw new DirectoryNotFoundException($"argumentName: {argumentName} fileName: {fileSystemInfo.FullName}");

        }

        [DebuggerStepThrough]
        public static void NotNullOrWhitespace(string @string, string argumentName)
        {
            if(string.IsNullOrWhiteSpace(@string))
                throw new ArgumentException(argumentName);
        }
    }
}