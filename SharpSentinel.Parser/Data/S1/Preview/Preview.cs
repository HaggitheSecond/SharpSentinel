namespace SharpSentinel.Parser.Data.S1.Preview
{
    public class Preview
    {
        /// <summary>
        /// The preredered .png quicklook
        /// </summary>
        public QuickLook QuickLook { get; set; }

        /// <summary>
        /// The .kml mapoverlay 
        /// </summary>
        public MapOverlay MapOverlay { get; set; }

        /// <summary>
        /// The .html product preview
        /// </summary>
        public ProductPreview ProductPreview { get; set; }
    }
}