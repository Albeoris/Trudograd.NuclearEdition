using System;
using HarmonyLib;
using UnityEngine;

namespace Trudograd.NuclearEdition
{
    /// <summary>
    /// Intercepts attempts to use a door to automatically unlock it.
    /// </summary>
    [HarmonyPatch(typeof(DoorComponent))]
    [HarmonyPatch(nameof(DoorComponent.Use))]
    internal sealed class DoorComponent_Use
    {
        static DoorComponent_Use()
        {
            Debug.Log($"[{nameof(NuclearEdition)}] Apply patch: {nameof(DoorComponent_Use)}");
        }
        
        public static Boolean Prefix(DoorComponent __instance, CharacterComponent character)
        {
            return LockerComponentOpener.Open(__instance, __instance.door.Prototype.lockerKey, character);
        }
    }
}