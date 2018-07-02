using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SharpSentinel.UI.Extensions
{
    public static class StringExtensions
    {
        public static string FormatXml(this string self)
        {
            try
            {
                return XDocument.Parse(self).ToString();
            }
            catch (Exception)
            {
                return self;
            }
        }
    }
}