using System;
using ABBG;
using HarmonyLib;
using UnityEngine;

namespace Trudograd.NuclearEdition
{
    /// <summary>
    /// Round ends automatically if the player has no available cards.
    /// </summary>
    [HarmonyPatch(typeof(PlayCardAction), "OnEndAction")]
    internal sealed class PlayCardAction_OnEndAction
    {
        static PlayCardAction_OnEndAction()
        {
            Debug.Log($"{nameof(NuclearEdition)} Apply patch: {nameof(PlayCardAction_OnEndAction)}");
        }

        public static void Postfix(PlayCardAction __instance)
        {
            if (!Configuration.Bombagan.PlayerRoundEndAuto)
                return;

            if (__instance.Board.Mode == ABBG.ABBG.Mode.Tutorial)
                return;

            if (__instance.Board.Side != Board.PlayerType.Hero)
                return;

            Validator validator = __instance.Board.Validator;

            foreach (Card card in __instance.Board.GetCards(Board.LocationType.PlayerHand))
            {
                validator.SetCard(card);
                if (validator.CanPlayCard() == 0)
                {
                    validator.Reset();
                    return;
                }
            }

            validator.Reset();

            __instance.Board.Main.AddCommand(new NextTurnCommand());
        }
    }
}