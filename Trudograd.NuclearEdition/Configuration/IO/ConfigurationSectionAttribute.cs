using System;

namespace Trudograd.NuclearEdition
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ConfigurationSectionAttribute : Attribute
    {
        public String Description { get; }

        public ConfigurationSectionAttribute(String description)
        {
            Description = description;
        }
    }
}