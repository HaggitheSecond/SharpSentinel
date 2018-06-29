using System.IO;
using System.Xml;
using JetBrains.Annotations;
using SharpSentinel.Parser.Data.Common;
using SharpSentinel.Parser.Data.S1;
using SharpSentinel.Parser.Helpers;

namespace SharpSentinel.Parser.Parsers.Annotations
{
    public static class CalibrationAnnotationParser
    {
        public static CalibriationAnnotation ParseCalibriationAnnotation([NotNull]FileInfo fileInfo, [CanBeNull] Checksum checkSum)
        {
            Guard.NotNullAndValidFileSystemInfo(fileInfo, nameof(fileInfo));

            var calibrationAnnotation = new CalibriationAnnotation
            {
                File = fileInfo,
                Checksum = checkSum
            };

            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open))
            {
                var document = new XmlDocument();
                document.Load(fileStream);

            }

            return calibrationAnnotation;
        }
    }
}