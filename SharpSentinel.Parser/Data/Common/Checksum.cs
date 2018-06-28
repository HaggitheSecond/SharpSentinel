namespace SharpSentinel.Parser.Data.Common
{
    public class Checksum
    {
        /// <summary>
        /// The name of the algorithm used to generate the checksum value.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The generated checksum.
        /// </summary>
        public string Sum { get; set; }
    }
}