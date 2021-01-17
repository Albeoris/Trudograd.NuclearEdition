using ABBG;
using HarmonyLib;
using UnityEngine;

namespace Trudograd.NuclearEdition
{
    /// <summary>
    /// Round ends automatically if the player has no available cards.
    /// </summary>
    [HarmonyPatch(typeof(ProcessOpponentInputAction), "OnEndAction")]
    internal sealed class ProcessOpponentInputAction_OnEndAction
    {
        static ProcessOpponentInputAction_OnEndAction()
        {
            Debug.Log($"[{nameof(NuclearEdition)}] Apply patch: {nameof(ProcessOpponentInputAction_OnEndAction)}");
        }

        public static void Postfix(ProcessOpponentInputAction __instance)
        {
            if (!Configuration.Bombagan.OpponentRoundEndAuto)
                return;

            if (__instance.Board.Mode == ABBG.ABBG.Mode.Tutorial)
                return;

            __instance.Board.Main.AddCommand(new NextTurnCommand());
        }
    }
}