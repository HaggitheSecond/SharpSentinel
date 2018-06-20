using System.Collections.Generic;
using System.Xml;
using SharpSentinel.Parser.Data.Internal.FileTypes;

namespace SharpSentinel.Parser.Data.ManifestObjects
{
    public sealed class Manifest : DataFile
    {
        /// <summary>
        /// Metadata describing the mission platform to which acquired the data.
        /// </summary>
        public Platform Platform { get; set; }

        /// <summary>
        /// Time extent of the Sentinel-1 L1 product. 
        /// </summary>
        public AcquisitionPeriod AcquisitionPeriod { get; set; }

        /// <summary>
        /// Contains information describing the orbit or the orbit range of the image data.
        /// </summary>
        public MeasurementOrbitReference MeasurementOrbitReference { get; set; }

        /// <summary>
        /// Geographical and time location of the instrument footprint, considered as a single frame.
        /// This element is present for all products except ASAR L2 OCN products which are generated from an ASAR L1 input.
        /// </summary>
        public MeasurementFrameSet MeasurementFrameSet { get; set; }

        /// <summary>
        /// Metadata describing the product.
        /// </summary>
        public GeneralProductInformation GeneralProductInformation { get; set; }

        /// <summary>
        /// Metadata describing the processing steps performed on the auxiliary data.

        /// </summary>
        public Processing Processing { get; set; }

        public Manifest(string filePath)
        : base(filePath)
        {
        }
    }
}