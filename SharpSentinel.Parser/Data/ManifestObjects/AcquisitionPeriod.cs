using System;
using System.Xml;

// ReSharper disable PossibleNullReferenceException

namespace SharpSentinel.Parser.Data.ManifestObjects
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
        public double StartTimeANX { get; set; }

        /// <summary>
        /// Sensing stop time of the input data relative to the ascending node crossing.This is a count of the time elapsed since the orbit ascending node crossing[ms].
        /// </summary>
        public double StopTimeANX { get; set; }
    }
}