using System.Collections.Generic;
using SharpSentinel.Parser.Data.S1;
using SharpSentinel.Parser.Parsers;

namespace SharpSentinel.Parser.Data
{
    public class S1Data : BaseData
    {
        public IList<MeasurementDataUnit> MeasurementDataUnits { get; set; }

        public MeasurementDataUnit QuickLookDataUnit { get; set; }
    }
}