using System;
using System.Collections.Generic;

namespace Trudograd.NuclearEdition
{
    [ConfigurationSection("Changes behavior when a character is holding a scanner in the main slot.")]
    public sealed class ScannerConfiguration
    {
        [ConfigurationValue("false", "[vanilla] empty containers will be highlighted")]
        [ConfigurationValue("true ", "[default] empty containers will not be highlighted")]
        public Boolean DoNotHighlightEmptyContainers { get; set; } = true;

        [ConfigurationValue("Trash Item 1; Trash Item 2", "semicolon-delimited list of localized item names that will not be highlighted")]
        public HashSet<String> DoNotHighlightItems { get; set; } = new HashSet<String>();
    }
}