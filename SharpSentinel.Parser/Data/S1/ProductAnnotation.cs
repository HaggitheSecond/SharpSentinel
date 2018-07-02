using System.IO;
using SharpSentinel.Parser.Data.Common;
using SharpSentinel.Parser.Data.Interfaces;
using SharpSentinel.Parser.Data.S1.Annotations;

namespace SharpSentinel.Parser.Data.S1
{
    public class ProductAnnotation : IFile, IXmlFile
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
        /// ADS header data set record. This DSR contains information that applies to the entire data set. 
        /// </summary>
        public AdsHeader AdsHeader { get; set; }

        /// <summary>
        /// Quality information data set record. This DSR contains the quality flags and the values used to set them during image processing as well as the overall quality index.
        /// </summary>
        public QualityInformation QualityInformation { get; set; }
    }
}