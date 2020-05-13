using System;

namespace Trudograd.NuclearEdition
{
    [ConfigurationSection("Changes the behavior in the Bombagan card game.")]
    public sealed class BombaganConfiguration
    {
        [ConfigurationValue("false", "[default] double click to play a card")]
        [ConfigurationValue("true ", "click once to play a card; you cannot drag cards")]
        public Boolean ClickOnceToPlayCard { get; set; }
        
        [ConfigurationValue("false", "[default] player must manually discard cards")]
        [ConfigurationValue("true ", "right-click on the card to discard it; replaces description view")]
        public Boolean RightClickToDiscardCard { get; set; }

        [ConfigurationValue("false", "[default] the player must manually press the completion button")]
        [ConfigurationValue("true ", "the player's round ends automatically if the player has no available cards")]
        public Boolean PlayerRoundEndAuto { get; set; }

        [ConfigurationValue("false", "[default] the player must manually press the completion button")]
        [ConfigurationValue("true ", "the opponent's round ends automatically")]
        public Boolean OpponentRoundEndAuto { get; set; }
    }
}