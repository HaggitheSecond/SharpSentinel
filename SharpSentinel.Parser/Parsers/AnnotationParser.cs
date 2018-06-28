using System.IO;
using JetBrains.Annotations;
using SharpSentinel.Parser.Data.Common;
using SharpSentinel.Parser.Data.S1;
using SharpSentinel.Parser.Helpers;

namespace SharpSentinel.Parser.Parsers
{
    public static class AnnotationParser
    {
        public static NoiseAnnotation ParseNoiseAnnotation([NotNull]FileInfo fileInfo, [CanBeNull]Checksum checkSum)
        {
            Guard.NotNullAndValidFileSystemInfo(fileInfo, nameof(fileInfo));

            var noiseAnnotation = new NoiseAnnotation
            {
                File = fileInfo,
                Checksum = checkSum
            };

            return noiseAnnotation;
        }

        public static ProductAnnotation ParseProductAnnotation([NotNull]FileInfo fileInfo, [CanBeNull]Checksum checkSum)
        {
            Guard.NotNullAndValidFileSystemInfo(fileInfo, nameof(fileInfo));

            var productAnnotation = new ProductAnnotation
            {
                File = fileInfo,
                Checksum = checkSum
            };

            return productAnnotation;
        }

        public static CalibriationAnnotation ParseCalibriationAnnotation([NotNull]FileInfo fileInfo, [CanBeNull] Checksum checkSum)
        {
            Guard.NotNullAndValidFileSystemInfo(fileInfo, nameof(fileInfo));

            var calibrationAnnotation = new CalibriationAnnotation
            {
                File = fileInfo,
                Checksum = checkSum
            };

            return calibrationAnnotation;
        }
    }
}