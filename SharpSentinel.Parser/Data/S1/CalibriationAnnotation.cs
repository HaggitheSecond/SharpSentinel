using System.Collections.Generic;
using System.IO;
using SharpSentinel.Parser.Data.Common;
using SharpSentinel.Parser.Data.Interfaces;
using SharpSentinel.Parser.Data.S1.Annotations;

namespace SharpSentinel.Parser.Data.S1
{
    public class CalibriationAnnotation : IXmlFile
    {
        /// <summary>
        /// Systemdata for the .xml file
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// The raw XML from the file
        /// </summary>
        public string RawXml { get; set; }

        /// <summary>
        /// The checksum included in the manifest.safe
        /// </summary>
        public Checksum Checksum { get; set; }

        /// <summary>
        /// ADS header data set record. This DSR contains information that applies to the entire data set
        /// </summary>
        public AdsHeader AdsHeader { get; set; }

        /// <summary>
        /// Swath dependent absolute calibration constant (Kabs). This value comes from the auxiliary input and is 
        /// built in to the absolute calibration vectors sigmaNought, betaNought and gamma.
        /// </summary>
        public double AbsoluteCalibrationConstant { get; set; }

        /// <summary>
        /// Calibration vector record. This record holds the calibration vectors and associated fields required to derive radiometrically calibrated imagery from the image MDS.With a minimum calibration vector update rate of
        /// 1s and a maximum product length of 25 minutes, the maximum size of this list is 1500 elements.The azimuth spacing used will be different for different modes and product types
        /// </summary>
        public IList<CalibrationVector> CalibrationVectors { get; set; }
    }
}