using System.ComponentModel;
using System;

namespace PWRekruter.Enums
{
    public static class EnumExtensions
    {
        public static string GetEnumLabel(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute?.Description ?? value.ToString();
        }
    }
}
