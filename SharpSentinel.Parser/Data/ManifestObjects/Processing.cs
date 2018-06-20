using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SharpSentinel.Parser.Data.ManifestObjects
{
    public class Processing
    {
        /// <summary>
        /// Name of the processing step used to create the product. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Processing start time.
        /// </summary>
        public DateTimeOffset Start { get; set; }

        /// <summary>
        /// Processing stop time
        /// </summary>
        public DateTimeOffset Stop { get; set; }

        /// <summary>
        /// Identifies an organisation authority of the processing step.
        /// </summary>
        public ProcessingFacility Facility { get; set; }

        /// <summary>
        /// Reference to resources involved in the processing. This  includes references to orbit and attitude files used to process the product.
        /// </summary>
        public IList<ProcessingResource> Resources { get; set; }

        public Processing()
        {
            this.Resources = new List<ProcessingResource>();
        }
    }
    
    public class ProcessingFacility
    {
        /// <summary>
        /// Name of the country where the facility is located. This element is configurable within the IPF.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Name of the facility where the processing step was performed. This element is configurable within the IPF.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the organisation responsible for the facility. This element is configurable within the IPF.
        /// </summary>
        public string Organisation { get; set; }

        /// <summary>
        /// Geographical location of the facility. This element is configurable within the IPF.
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Reference to the software used for the processing step.
        /// </summary>
        public IList<ProcessingSoftware> Software { get; set; }

        public ProcessingFacility()
        {
            this.Software = new List<ProcessingSoftware>();
        }
    }

    public class ProcessingSoftware
    {
        /// <summary>
        /// Name of the software.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Software version identification. 
        /// </summary>
        public string Version { get; set; }
    }

    public class ProcessingResource
    {
        /// <summary>
        /// Name of the resource.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Role the resource played in processing.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// URL of the resource. 
        /// </summary>
        public Uri Href { get; set; }

        /// <summary>
        /// Metadata describing the processing steps performed on the auxiliary data by the resource.A resource can have its
        /// own processing metadata entry that may contain resources with their own processing entries.Each additional
        /// resource and processing entry is nested within the previous entry.For the IPF, the entries can be “SLC Processing”,
        /// “Post Processing” or “L2 Processing”.
        /// </summary>
        public Processing Processing { get; set; }
    }
}