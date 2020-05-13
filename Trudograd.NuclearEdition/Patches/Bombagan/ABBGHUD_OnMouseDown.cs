using System;
using System.Runtime.CompilerServices;
using ABBG;
using HarmonyLib;
using UnityEngine;

namespace Trudograd.NuclearEdition
{
    /// <summary>
    /// Intercepts a mouse click, and plays a card instead of dragging it.
    /// </summary>
    [HarmonyPatch(typeof(ABBGHUD))]
    [HarmonyPatch(nameof(ABBGHUD.OnMouseDown))]
    internal sealed class ABBGHUD_OnMouseDown
    {
        static ABBGHUD_OnMouseDown()
        {
            Debug.Log($"{nameof(NuclearEdition)} Apply patch: {nameof(ABBGHUD_OnMouseDown)}");
        }

        public static Boolean Prefix(ABBGHUD __instance, DragObject dragObject, Boolean leftButton)
        {
            return leftButton
                ? ProcessLeftMouseButton(__instance, dragObject)
                : ProcessRightMouseButton(__instance, dragObject);
        }

        private static Boolean ProcessLeftMouseButton(ABBGHUD __instance, DragObject dragObject)
        {
            if (!Configuration.Bombagan.ClickOnceToPlayCard)
                return HarmonyPrefixResult.CallOriginal;

            if (__instance.Main.Board.Mode == ABBG.ABBG.Mode.Tutorial)
                return HarmonyPrefixResult.CallOriginal;

            // Discard cards
            if (__instance.Main.Board.SelectedCards.IsMultiselectMode())
                return HarmonyPrefixResult.CallOriginal;

            if (dragObject is CardHUD)
            {
                __instance.OnDoubleClick(dragObject);
                return HarmonyPrefixResult.SkipOriginal;
            }

            return HarmonyPrefixResult.CallOriginal;
        }

        private static Boolean ProcessRightMouseButton(ABBGHUD __instance, DragObject dragObject)
        {
            if (!Configuration.Bombagan.RightClickToDiscardCard)
                return HarmonyPrefixResult.CallOriginal;

            var main = __instance.Main;
            if (main.Board.Mode == ABBG.ABBG.Mode.Tutorial)
                return HarmonyPrefixResult.CallOriginal;

            Selection selectedCards = main.Board.SelectedCards;

            // Discard cards mode
            if (selectedCards.IsMultiselectMode())
                return HarmonyPrefixResult.CallOriginal;

            if (main.Board.GetAblilityToDrawCards() != Board.PlayerType.Hero)
                return HarmonyPrefixResult.CallOriginal;

            if (dragObject is CardHUD card)
            {
                main.AddCommand(new DrawCardsModeCommand(Board.LocationType.PlayerHand));
                main.Board.SelectedCards.Toggle(card.Item);
                main.AddCommand(new DrawCardsCommand());
            }

            return HarmonyPrefixResult.SkipOriginal;
        }
    }
}