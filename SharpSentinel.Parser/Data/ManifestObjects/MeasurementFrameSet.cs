using System.Collections.Generic;

namespace SharpSentinel.Parser.Data.ManifestObjects
{
    public class MeasurementFrameSet
    {
        /// <summary>
        /// The instrument footprint frame. There is one frame per product for SM, IW and EW modes, and one frame per vignette for WV mode products.
        /// </summary>
        public IList<MeasurementFrame> MeasurementFrames { get; set; }

        public MeasurementFrameSet()
        {
            this.MeasurementFrames = new List<MeasurementFrame>();
        }
    }

    public class MeasurementFrame
    {
        /// <summary>
        /// Number of the WV vignette which this frame describes.
        /// </summary>
        public int? Number { get; set; }

        // TODO: Find or create gml type
        /// <summary>
        /// Coordinates of instrument footprint in GML notation (gml:coordinates type as defined in http://www.opengis.net/gml, 
        /// namely string with 4 pairs of coordinates(lon, lat of near and far range at start and stop time of the image) separated by a space.
        /// </summary>
        public string Footprint { get; set; }
    }
}