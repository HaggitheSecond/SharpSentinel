using System.IO;

namespace SharpSentinel.Parser.Data.S1
{
    public class MeasurementDataUnit
    {
        public FileInfo File { get; set; }

        public string ChecksumName { get; set; }

        public string Checksum { get; set; }

        public MeasurementDataUnitType MeasurementDataUnitType { get; set; }
    }

    public enum MeasurementDataUnitType
    {
        Measurement,
        QuickLook
    }
}