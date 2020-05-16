using System;
using System.Globalization;

namespace Trudograd.NuclearEdition
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class ConfigurationValueAttribute : Attribute
    {
        public String Value { get; }
        public String Description { get; }

        public ConfigurationValueAttribute(String value, String description)
        {
            Value = value;
            Description = description;
        }
        
        public ConfigurationValueAttribute(Object value, String description)
        {
            Value = value is Enum ? Convert.ToInt64(value).ToString(CultureInfo.InvariantCulture) : value.ToString();
            Description = description;
        }
    }
}