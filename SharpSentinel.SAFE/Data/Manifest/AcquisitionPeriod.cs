using System;

namespace SharpSentinel.Parser.Data.Manifest
{
    public class AcquisitionPeriod
    {
        /// <summary>
        /// Sensing start time of the input data used to produce the output image.
        /// </summary>
        public DateTimeOffset StartTime { get; set; }

        /// <summary>
        /// Sensing stop time of the input data used to produce the output image.
        /// </summary>
        public DateTimeOffset StopTime { get; set; }

        /// <summary>
        /// Sensing start time of the input data relative to the ascending node crossing.This is a count of the time elapsed since the orbit ascending node crossing[ms].
        /// </summary>
        public decimal StartTimeANX { get; set; }

        /// <summary>
        /// Sensing stop time of the input data relative to the ascending node crossing.This is a count of the time elapsed since the orbit ascending node crossing[ms].
        /// </summary>
        public decimal StopTimeANX { get; set; }
    }
}