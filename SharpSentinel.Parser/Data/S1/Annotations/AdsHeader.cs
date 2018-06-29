using System;
using System.Collections.Generic;
using SharpSentinel.Parser.Data.ManifestObjects;

namespace SharpSentinel.Parser.Data.S1.Annotations
{
    public class AdsHeader
    {
        /// <summary>
        /// Mission identifier for this data set. 
        /// </summary>
        public string MissionId { get; set; }

        /// <summary>
        /// Product type for this data set. 
        /// </summary>
        public ProductTypeType ProductType { get; set; }

        /// <summary>
        /// Polarisation for this data set. 
        /// </summary>
        public TransmitterReceiverPolarisationType Polarisation { get; set; }

        /// <summary>
        /// Sensor mode for this data set
        /// </summary>
        public InstrumentModeType Mode { get; set; }

        /// <summary>
        /// Swath identifier for this data set. This element identifies the swath that applies to all data contained within this data set.The swath identifier "EW" is
        /// used for products in which the 5 EW swaths have been merged.Likewise, "IW" is used for products in which the 3 IW swaths have been merged.
        /// </summary>
        public SwathType Swath { get; set; }

        /// <summary>
        /// Zero Doppler start time of the output image [UTC].
        /// </summary>
        public DateTimeOffset StartTime { get; set; }

        /// <summary>
        /// Zero Doppler stop time of the output image [UTC].
        /// </summary>
        public DateTimeOffset StopTime { get; set; }

        /// <summary>
        /// Absolute orbit number at data set start time. 
        /// </summary>
        public int AbsoluteOrbitNumber { get; set; }

        /// <summary>
        /// Mission data take identifier. 
        /// </summary>
        public int MissionDataTakeId { get; set; }

        /// <summary>
        /// Image number. For WV products the image number is used to distinguish between vignettes.For SM, IW and EW modes the image number is still used
        /// but refers instead to each swath and polarisation combination(known as the 'channel') of the data.
        /// </summary>
        public int ImageNumber { get; set; }
    }
}