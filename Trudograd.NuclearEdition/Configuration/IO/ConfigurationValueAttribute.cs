using System;

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
    }
}