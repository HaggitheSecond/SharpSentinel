using System.IO;
using SharpSentinel.Parser.Data.Common;

namespace SharpSentinel.Parser.Data.S1
{
    public class MeasurementDataUnit
    {
        /// <summary>
        /// Systemdata for the .tiff file
        /// </summary>
        public FileInfo File { get; set; }
        
        /// <summary>
        /// The checksum included in the manifest.safe
        /// </summary>
        public Checksum Checksum { get; set; }

        /// <summary>
        /// The type of dataUnit, either a quicklook or an actual measurement
        /// </summary>
        public MeasurementDataUnitType MeasurementDataUnitType { get; set; }

        /// <summary>
        /// The calibration data set contains calibration information and the  beta nought, sigma nought, gamma and digital number(DN)
        /// Look-up Tables(LUT) that can be used for absolute product  calibration.
        /// This is not present in Quicklook-Data.
        /// </summary>
        public CalibriationAnnotation CalibriationAnnotation { get; set; }

        /// <summary>
        /// The noise data set contains the estimated thermal noise LUT. 
        /// This is not present in Quicklook-Data.
        /// </summary>
        public NoiseAnnotation NoiseAnnotation { get; set; }

        /// <summary>
        /// The Level 1 product annotation data set contains the metadata that describes the main characteristics of the product such as:
        /// state of the platform during acquisition, image properties, Doppler information, geographic location, etc.
        /// This is not present in Quicklook-Data.
        /// </summary>
        public ProductAnnotation ProductAnnotation { get; set; }
    }

    public enum MeasurementDataUnitType
    {
        Measurement,
        QuickLook
    }
}