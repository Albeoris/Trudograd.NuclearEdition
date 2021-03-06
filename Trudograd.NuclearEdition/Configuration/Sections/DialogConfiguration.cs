﻿using System;

namespace Trudograd.NuclearEdition
{
    [ConfigurationSection("Changes the behavior of dialogs in the game.")]
    public sealed class DialogConfiguration
    {
        [ConfigurationValue(DialogChanceRepresentation.DoNotDisplay, "[default] chance will not be indicated")]
        [ConfigurationValue(DialogChanceRepresentation.Segments, "chance will be indicated by color segments")]
        [ConfigurationValue(DialogChanceRepresentation.Gradient, "chance will be indicated by color gradient")]
        [ConfigurationValue(DialogChanceRepresentation.Percent, "chance will be indicated as a percentage")]
        [ConfigurationValue(DialogChanceRepresentation.Value, "exact requirements will be indicated")]
        public DialogChanceRepresentation DisplayChanceSuccess { get; set; }

        [ConfigurationValue("20", "[default] absolute value of the inaccuracy")]
        public Int32 DisplayChanceAbsoluteInaccuracy { get; set; } = 20;

        [ConfigurationValue("0", "[default] relative (%) value of the inaccuracy")]
        public Int32 DisplayChanceRelativeInaccuracy { get; set; }
    }

    public enum DialogChanceRepresentation
    {
        DoNotDisplay = 0,
        Segments = 1,
        Gradient = 2,
        Percent = 3,
        Value = 4
    }
}