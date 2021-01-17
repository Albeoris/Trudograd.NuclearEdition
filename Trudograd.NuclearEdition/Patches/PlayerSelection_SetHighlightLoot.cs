using System;
using HarmonyLib;
using UnityEngine;

namespace Trudograd.NuclearEdition
{
    /// <summary>
    /// Allows to turn on the highlight mode if the scanner is working.
    /// </summary>
    [HarmonyPatch(typeof(PlayerSelection))]
    [HarmonyPatch(nameof(PlayerSelection.SetHighlightLoot))]
    internal sealed class PlayerSelection_SetHighlightLoot
    {
        static PlayerSelection_SetHighlightLoot()
        {
            Debug.Log($"[{nameof(NuclearEdition)}] Apply patch: {nameof(PlayerSelection_SetHighlightLoot)}");
        }

        public static Boolean Prefix(PlayerSelection __instance, PlayerSelection.Mode ____mode, Boolean isEnabled)
        {
            if (isEnabled && ____mode == PlayerSelection.Mode.Scanner)
                __instance.Reset();

            return HarmonyPrefixResult.CallOriginal;
        }
    }
}