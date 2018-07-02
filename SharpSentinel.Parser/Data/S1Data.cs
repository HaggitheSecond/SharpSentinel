using System.Collections.Generic;
using SharpSentinel.Parser.Data.S1;
using SharpSentinel.Parser.Data.S1.Preview;
using SharpSentinel.Parser.Parsers;

namespace SharpSentinel.Parser.Data
{
    public class S1Data : BaseData
    {
        /// <summary>
        /// Collection of measurementDataUnits with corresponding noise-, calibration- and productannotation
        /// </summary>
        public IList<MeasurementDataUnit> MeasurementDataUnits { get; set; }
        
        /// <summary>
        /// The report file
        /// </summary>
        public ReportFile ReportFile { get; set; }

        /// <summary>
        /// The previewelements: mapoverlay, quicklook and productpreview
        /// </summary>
        public Preview Preview { get; set; }
    }
}