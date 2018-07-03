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

        public static bool ContainsOnlyWhitespaceAndNewLines(this string self)
        {
            var text1 = self.Replace(" ", "");
            var text2 = text1.Replace("\r\n", "");

            return text2.Length == 0;
        }
    }
}