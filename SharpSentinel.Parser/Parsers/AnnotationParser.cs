using System.IO;
using SharpSentinel.Parser.Data.Common;
using SharpSentinel.Parser.Data.S1;

namespace SharpSentinel.Parser.Parsers
{
    public static class AnnotationParser
    {
        public static NoiseAnnotation ParseNoiseAnnotation(FileInfo fileInfo, Checksum checkSum)
        {
            if (File.Exists(fileInfo.FullName) == false)
                throw new FileNotFoundException(fileInfo.Name);

            var noiseAnnotation = new NoiseAnnotation
            {
                File = fileInfo,
                Checksum = checkSum
            };

            return noiseAnnotation;
        }

        public static ProductAnnotation ParseProductAnnotation(FileInfo fileInfo, Checksum checkSum)
        {
            if (File.Exists(fileInfo.FullName) == false)
                throw new FileNotFoundException(fileInfo.Name);

            var productAnnotation = new ProductAnnotation
            {
                File = fileInfo,
                Checksum = checkSum
            };

            return productAnnotation;
        }

        public static CalibriationAnnotation ParseCalibriationAnnotation(FileInfo fileInfo, Checksum checkSum)
        {
            if (File.Exists(fileInfo.FullName) == false)
                throw new FileNotFoundException(fileInfo.Name);

            var calibrationAnnotation = new CalibriationAnnotation
            {
                File = fileInfo,
                Checksum = checkSum
            };

            return calibrationAnnotation;
        }
    }
}