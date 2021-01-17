using System;
using HarmonyLib;
using UnityEngine;

namespace Trudograd.NuclearEdition
{
    /// <summary>
    /// Enables tracking of the character while holding down the "Move camera to the character" key.
    /// </summary>
    [HarmonyPatch(typeof(PlayerControl))]
    [HarmonyPatch("get_" + nameof(PlayerControl.CameraAlwaysSnap))]
    internal sealed class PlayerSelection_GetCameraAlwaysSnap
    {
        static PlayerSelection_GetCameraAlwaysSnap()
        {
            Debug.Log($"[{nameof(NuclearEdition)}] Apply patch: {nameof(PlayerSelection_GetCameraAlwaysSnap)}");
        }

        public static void Postfix(ref Boolean __result)
        {
            if (__result)
                return;
            
            if (InputManager.GetKey(InputManager.Action.Cam2Player))
                __result = true;
        }
    }
}