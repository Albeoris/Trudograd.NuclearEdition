using System;
using HarmonyLib;
using UnityEngine;

namespace Trudograd.NuclearEdition
{
    /// <summary>
    /// Adds the ability to specify a custom color for highlighting.
    /// </summary>
    [HarmonyPatch(typeof(PlayerSelection))]
    [HarmonyPatch("GetColorBySelection")]
    internal sealed class PlayerSelection_GetColorBySelection
    {
        static PlayerSelection_GetColorBySelection()
        {
            Debug.Log($"[{nameof(NuclearEdition)}] Apply patch: {nameof(PlayerSelection_GetColorBySelection)}");
        }

        public static Boolean Prefix(PlayerSelection __instance, PlayerSelection.SelectionType type, ref Color __result)
        {
            UInt32 colorBits = (UInt32) type;
            if (colorBits < 0x01000000)
                return HarmonyPrefixResult.CallOriginal;

            Single a = ((colorBits & 0xFF_00_00_00) >> 24) / 256.0f;
            Single r = ((colorBits & 0x00_FF_00_00) >> 16) / 256.0f;
            Single g = ((colorBits & 0x00_00_FF_00) >> 8) / 256.0f;
            Single b = (colorBits & 0x00_00_00_FF) / 256.0f;

            __result = new Color(r, g, b, a);
            return HarmonyPrefixResult.SkipOriginal;
        }
    }
}