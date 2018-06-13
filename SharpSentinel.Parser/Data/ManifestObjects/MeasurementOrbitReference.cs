using System;

namespace SharpSentinel.Parser.Data.ManifestObjects
{
    public class MeasurementOrbitReference
    {
        /// <summary>
        /// Absolute orbit number of the oldest line within the image data.
        /// </summary>
        public int OrbitNumberStart { get; set; }

        /// <summary>
        /// Absolute orbit number of the most recent line within the image data.
        /// </summary>
        public int OrbitNumberStop { get; set; }

        /// <summary>
        /// Relative orbit number of the oldest line within the image data.
        /// </summary>
        public int RelativeOrbitNumberStart { get; set; }

        /// <summary>
        /// Relative orbit number of the most recent line within the image data.
        /// </summary>
        public int RelativeOrbitNumberStop { get; set; }

        /// <summary>
        /// Absolute sequence number of the mission cycle to which the oldest image data applies.
        /// </summary>
        public int CycleNumber { get; set; }

        /// <summary>
        /// Id of the mission phase to which the oldest image data applies.
        /// </summary>
        public int PhaseIdentifier { get; set; }

        /// <summary>
        /// Direction of the orbit (ascending, descending) for the oldest image data in the product(the start of the product).
        /// </summary>
        public PassType Pass { get; set; }

        /// <summary>
        /// UTC time of the ascending node of the orbit. This element is present for all products except ASAR L2 OCN products which are generated from an ASAR L1 input.
        /// </summary>
        public DateTimeOffset AscendingNodeTime { get; set; }
    }

    public enum PassType
    {
        Ascending,
        Descending
    }
}