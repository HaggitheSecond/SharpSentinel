using System;
using System.Collections.Generic;

namespace SharpSentinel.Parser.Data.S1.Annotations
{
    public class NoiseRangeVector
    {
        /// <summary>
        /// Zero Doppler azimuth time at which noise vector applies. 
        /// </summary>
        public DateTimeOffset AzimuthTime { get; set; }

        /// <summary>
        /// Image line at which the noise vector applies.
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// Comibination of pixel from pixelArray and noiseRangeLut from noiseRangeLutArray
        /// </summary>
        public IList<NoiseRangeVectorValue> Values { get; set; }
    }

    public class NoiseRangeVectorValue
    {
        /// <summary>
        /// Image pixel at which the noise vector applies.
        /// </summary>
        public int Pixel { get; set; }

        /// <summary>
        /// Range thermal noise correction vector power value. 
        /// </summary>
        public double NoiseRangeLut { get; set; }
    }
}