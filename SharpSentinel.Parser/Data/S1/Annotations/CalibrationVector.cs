using System;
using System.Collections.Generic;

namespace SharpSentinel.Parser.Data.S1.Annotations
{
    public class CalibrationVector
    {
        /// <summary>
        /// Zero Doppler azimuth time at which calibration vector applies. 
        /// </summary>
        public DateTimeOffset AzimuthTime { get; set; }

        /// <summary>
        /// Image line at which the calibration vector applies.
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// Comibination of pixel from pixelArray, sigmaNought from sigmaNoughtArray, betaNought from betaNoughtArry, gamma from gammaArray and dn from dnArray
        /// </summary>
        public IList<CalibratinoVectorValue> Values { get; set; }
    }

    public class CalibratinoVectorValue
    {
        /// <summary>
        /// Image pixel at which the calibration vector applies. 
        /// </summary>
        public int Pixel { get; set; }

        /// <summary>
        /// Sigma nought calibration vector. 
        /// </summary>
        public double SigmaNought { get; set; }

        /// <summary>
        /// Beta nought calibration vector.
        /// </summary>
        public double BetaNought { get; set; }

        /// <summary>
        /// Gamma calibration vector. 
        /// </summary>
        public double Gamma { get; set; }

        /// <summary>
        /// Digital number calibration vector. 
        /// </summary>
        public double Dn { get; set; }
    }
}