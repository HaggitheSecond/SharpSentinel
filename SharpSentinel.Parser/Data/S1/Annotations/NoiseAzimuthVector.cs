﻿using System.Collections.Generic;
using SharpSentinel.Parser.Data.ManifestObjects;

namespace SharpSentinel.Parser.Data.S1.Annotations
{
    public class NoiseAzimuthVector
    {
        /// <summary>
        /// Swath to which the noise vector applies.
        /// </summary>
        public SwathType Swath { get; set; }

        /// <summary>
        /// The first line at which this annotation applies.
        /// </summary>
        public int? FirstAzimuthLine { get; set; }

        /// <summary>
        /// The first sample at which this annotation applies.
        /// </summary>
        public int? FirstRangeSample { get; set; }

        /// <summary>
        /// The last line at which this annotation applies.
        /// </summary>
        public int? LastAzimuthLine { get; set; }

        /// <summary>
        /// The last sample at which this annotation applies.
        /// </summary>
        public int? LastRangeSample { get; set; }

        /// <summary>
        /// Comibination of line from lineArray and noiseAzimuthLut from noiseAzimuthLutArray
        /// </summary>
        public IList<NoiseAzimuthVectorValue> Values { get; set; }
    }

    public class NoiseAzimuthVectorValue
    {
        /// <summary>
        /// Image line at which the noise vector applies. 
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// Azimuth thermal noise correction vector power value.
        /// </summary>
        public double NoiseAzimuthLut { get; set; }
    }
}