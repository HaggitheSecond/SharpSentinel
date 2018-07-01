using System;
using System.Security.AccessControl;

namespace SharpSentinel.UI.Resources
{
    public static class ResourceLinks
    {
        private static Uri Create(string fileName)
        {
            return new Uri("pack://application:,,,/Resources/Images/" + fileName);
        }

        public static Uri Satellite => Create("satellite-variant.png");
    }
}