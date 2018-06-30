using System.Collections.Generic;
using System.ComponentModel;
using SharpSentinel.Parser.Extensions;

namespace SharpSentinel.Parser.Resources
{
    /// <summary>
    /// This file contains abbreviations for all products - I can't guarantee their correctness!
    /// </summary>
    public static class Abbreviations
    {
        public static string GetDescription(Abbreviation abbreviation)
        {
            return abbreviation.GetDescription();
        }

        public static string GetDescription(XMLAbbreviation abbreviation)
        {
            return abbreviation.GetDescription();
        }

        /// <summary>
        /// Gerneral sentinel related abbreviations
        /// </summary>
        public enum Abbreviation
        {
            [Description("Annotation Data Set")]
            ADS,
            [Description("ADSR")]
            ADSR,
            [Description("Instrument Processing Facility")]
            IPF,
            [Description("Single Look Complex")]
            SLC,
            [Description("Geo-reference Tag Image File Format")]
            GeoTIFF,
            [Description("Global Monitoring for Environment and Security")]
            GMES,
            [Description("Geophysical Model Function")]
            GMF,
            [Description("GRD")]
            GRD,
            [Description("Horizontal polarisation (Tx & Rx)")]
            HH,
            [Description("High Resolution")]
            HR,
            [Description("Medium Resolution")]
            MR,
            [Description("Horizontal Vertical polarisation")]
            HV,
            [Description("In-phase/Quadrature")]
            IQ,
            [Description("Interface Control Document")]
            ICD,
            [Description("Integrated Slide Lobe Ratio")]
            ISLR,
            [Description("Instrument Source Packet")]
            ISP,
            [Description("Interferometric Wide Swath")]
            IW,
            [Description("Look-up Table")]
            LUT,
            [Description("Measurement Data Set")]
            MDS,
            [Description("Normalised Radar Cross Section")]
            NRCS,
            [Description("L2 Ocean Product")]
            OCN,
            [Description("Ocean Swell Spectra")]
            OSW,
            [Description("Ocean Wind Field")]
            OWI,
            [Description("Preliminary Design Review")]
            PDR
        }
        
        /// <summary>
        /// XML related abbreviations
        /// </summary>
        public enum XMLAbbreviation
        {
            [Description("Description Information")]
            DMD,
            [Description("Preservation Information")]
            PDI,
            [Description("Representation Information")]
            REP
        }
    }
}