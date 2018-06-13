using System;
using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace SharpSentinel.Parser.Data.ManifestObjects
{
    public class GeneralProductInformation
    {
        /// <summary>
        /// The instrument configuration ID (Radar database ID) for this data.
        /// </summary>
        public int InstrumentConfigurationID { get; set; }

        /// <summary>
        /// Unique ID of the datatake within the mission. 
        /// </summary>
        public int MissionDataTakeID { get; set; }

        /// <summary>
        /// Transmit/Receive polarisation for the data. There is one element for each Tx/Rx combination.
        /// </summary>
        public IList<TransmitterReceiverPolarisationType> TransmitterReceiverPolarisation { get; set; }

        /// <summary>
        /// Output product class “A” for Annotation or “S” for Standard.
        /// </summary>
        public ProductClassType ProductClass { get; set; }

        /// <summary>
        /// Textual description of the output product class.
        /// </summary>
        public ProductClassDescriptionType ProductClassDescription { get; set; }

        /// <summary>
        /// The composition type of this product.
        /// </summary>
        public ProductCompositionType ProductComposition { get; set; }

        /// <summary>
        /// The product type (correction level) of this product. 
        /// </summary>
        public ProductTypeType ProductType { get; set; }

        /// <summary>
        /// Describes the required timeliness of the processing.
        /// </summary>
        public ProductTimelinessCategoryType ProductTimelinessCategory { get; set; }

        /// <summary>
        /// True if this is a slice from a larger product or false if this is a complete product.
        /// </summary>
        public bool SlideProductFlag { get; set; }

        /// <summary>
        /// Sensing start time of the segment to which this slice belongs.This field is only present if sliceProductFlag = true.
        /// </summary>
        public DateTimeOffset? SegementStartTime { get; set; }

        /// <summary>
        /// Absolute slice number of this slice starting at 1. This field is only present if sliceProductFlag = true.
        /// </summary>
        public int? SliceNumber { get; set; }

        /// <summary>
        /// Total number of slices in the complete data take. This field is only present if sliceProductFlag = true.
        /// </summary>
        public int? TotalSlices { get; set; }
    }

    /// <summary>
    /// Describes the required timeliness of the processing.
    /// </summary>
    public enum ProductTimelinessCategoryType
    {
        NRT10m,
        NRT1h,
        NRT3h,
        Fast24h,
        Offline,
        Reprocessing
    }

    /// <summary>
    /// Product type of product. 
    /// </summary>
    public enum ProductTypeType
    {
        SLC,
        GRD,
        OCN
    }

    /// <summary>
    /// Enumeration of the product composition types.
    /// </summary>
    public enum ProductCompositionType
    {
        Individial, 
        Slice,
        Assembled
    }

    /// <summary>
    /// Textual descriptions of product classes.
    /// </summary>
    public enum ProductClassDescriptionType
    {
        SARStandardL1Product,
        SARAnnotationL1Product,
        SARStandardL2Product,
        SARAnnotationL2Product
    }

    /// <summary>
    /// Enumeration of the product classes.
    /// </summary>
    public enum ProductClassType
    {
        A,
        S
    }

    /// <summary>
    /// Polarization of the data segment contained in a Data Object.
    /// </summary>
    public enum TransmitterReceiverPolarisationType
    {
        HH,
        VV,
        HV,
        VH
    }
}