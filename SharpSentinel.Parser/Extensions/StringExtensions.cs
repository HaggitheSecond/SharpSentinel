using System.Runtime.CompilerServices;

namespace SharpSentinel.Parser.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveWhitespaces(this string self)
        {
            return self.Replace(" ", "");
        }
    }
}