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

        public enum Abbreviation
        {
            [Description("Information Processing Facility")]
            IPF,
            [Description("Single Look Complex")]
            SLC ,
        }

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