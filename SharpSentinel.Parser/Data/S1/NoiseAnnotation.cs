using System.Collections.Generic;
using System.IO;
using SharpSentinel.Parser.Data.Common;
using SharpSentinel.Parser.Data.S1.Annotations;

namespace SharpSentinel.Parser.Data.S1
{
    public class NoiseAnnotation
    { 
        /// <summary>
        /// Systemdata for the .xml file
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// The raw XML from the file
        /// </summary>
        public string RawXML { get; set; }

        /// <summary>
        /// The checksum included in the manifest.safe
        /// </summary>
        public Checksum Checksum { get; set; }

        /// <summary>
        /// ADS header data set record. This DSR contains information that applies to the entire data set. 
        /// </summary>
        public AdsHeader AdsHeader { get; set; }

        /// <summary>
        /// Range noise vector list. This element is a list of noiseRangeVector records that contain the range thermal noise estimation
        /// for the image MDS.The list contains an entry for each update made along azimuth.
        /// </summary>
        public IList<NoiseRangeVector> NoiseRangeVectors { get; set; }

        /// <summary>
        /// Azimuth noise vector list. This annotation divides the image in blocks providing a list of azimuth noise vector records 
        /// that contain the thermal noise estimation for the block.The block belongs to a (sub-)swath
        /// (i.e.it can't cross by design two swaths) and it is delimited by firstAzimuthLine, lastAzimuthLine,firstRangeSample, lastRangeSample.
        /// </summary>
        public IList<NoiseAzimuthVector> NoiseAzimuthVectors { get; set; }
    }
}