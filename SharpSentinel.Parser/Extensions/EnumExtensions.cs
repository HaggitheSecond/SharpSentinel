using System;
using System.ComponentModel;
using SharpSentinel.Parser.Resources;

namespace SharpSentinel.Parser.Extensions
{
    public static class EnumExtensions
    {
        // TODO: Use c# 7.3 enum contraints once available
        public static string GetDescription<T>(this T self) where T : struct, IConvertible
        {
            if(self is Enum == false)
                throw new ArgumentException("Must be of enum-type");

            var type = self.GetType();

            var memInfo = type.GetMember(type.GetEnumName(self));
            var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return ((DescriptionAttribute) descriptionAttributes[0]).Description;
        }
    }
}