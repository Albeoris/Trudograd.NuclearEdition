using System;
using HarmonyLib;
using UnityEngine;

namespace Trudograd.NuclearEdition
{
    /// <summary>
    /// Intercepts attempts to use a chest to automatically unlock it.
    /// </summary>
    [HarmonyPatch(typeof(ChestComponent))]
    [HarmonyPatch(nameof(ChestComponent.Use))]
    internal sealed class ChestComponent_Use
    {
        static ChestComponent_Use()
        {
            Debug.Log($"{nameof(NuclearEdition)} Apply patch: {nameof(ChestComponent_Use)}");
        }
        
        public static Boolean Prefix(ChestComponent __instance, CharacterComponent character)
        {
            return LockerComponentOpener.Open(__instance, __instance.chest.Prototype.lockerKey, character);
        }
    }
}