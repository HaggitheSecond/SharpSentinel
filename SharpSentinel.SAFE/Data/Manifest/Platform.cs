using System;
using System.Collections.Generic;

namespace SharpSentinel.Parser.Data.Manifest
{
    public class Platform
    {
        /// <summary>
        /// Univocally identifies the mission according to standard defined by the World Data Center for Satellite Information (WDC-SI), 
        /// available at http://nssdc.gsfc.nasa.gov/nmc/scquery.html
        /// </summary>
        public string NssdcIdentifier { get; set; }

        /// <summary>
        /// The full mission name. E.g. “SENTINEL-1” 
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// The alphanumeric identifier of the platform within the mission.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Information related to the instrument on the platform to which acquired the data.

        /// </summary>
        public PlatformInstrument Instrument { get; set; }

        /// <summary>
        ///   Information on the leap second applied to the product UTC
        ///   timing. This element is only present if it was present in
        ///   the input L0 product, and if so the leapSeconInformation
        ///   information in the L0 product is copied into the L1/L2
        ///   product.
        /// </summary>
        public LeapSecondInformation LeapSecondInformation { get; set; }
    }

    public class LeapSecondInformation
    {
        /// <summary>
        /// UTC time of occurrence of leap second (if leap second occurred in the product time window); it represents the time after the leap second occurrence(i.e.midnight of day after the leap second)
        /// </summary>
        public DateTimeOffset UtcTimeOfOccurence { get; set; }

        /// <summary>
        /// Sign of leap second (+ or -).
        /// </summary>
        public SignType Sign { get; set; }

        public enum SignType
        {
            Plus,
            Minus
        }
    }

    public class PlatformInstrument
    {
        /// <summary>
        /// Instrument name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Abbreviated instrument name.
        /// </summary>
        public string Abbreviation { get; set; }

        /// <summary>
        /// Instrument mode.
        /// </summary>
        public InstrumentModeType Mode { get; set; }

        //TODO: Use actual type for this
        /// <summary>
        /// List of the swaths contained within a product. Most products will contain only one swath, except for TOPS SLC products which include 3 or 5 swaths.
        /// </summary>
        public IList<SwathType> Swaths { get; set; }
    }

    /// <summary>
    /// Instrument mode used to acquire the data segment.
    /// </summary>
    public enum InstrumentModeType
    {
        SM,
        IW,
        EW,
        WV,
        EN,
        AN,
        IM
    }

    /// <summary>
    /// Enumeration of all valid swath identifiers  for the Sentinel-1 SAR instrument.The
    /// S1-S6 swaths apply to SM products, the  IW and IW1-3 swaths apply to IW
    /// products (IW is used for detected IW  products where the 3 swaths are merged
    /// into one image), the EW and EW1-5  swaths apply to EW products(EW is
    /// used for detected EW products where the  5 swaths are merged into one image),
    /// and the WV1-2 swaths apply to WV  products.The EN and N1-N6 beams
    /// apply to the notch acquisition modes.  The IS1-IS7 swaths apply to ASAR IM
    /// and WV products.
    /// </summary>
    public enum SwathType
    {
        S1,
        S2,
        S3,
        S4,
        S5,
        S6,
        IW,
        IW1,
        IW2,
        IW3,
        EW,
        EW1,
        EW2,
        EW3,
        EW4,
        EW5,
        WV,
        WV1,
        WV2,
        EN,
        N1,
        N2,
        N3,
        N4,
        N5,
        N6,
        IS1,
        IS2,
        IS3,
        IS4,
        IS5,
        IS6,
        IS7
    }
}