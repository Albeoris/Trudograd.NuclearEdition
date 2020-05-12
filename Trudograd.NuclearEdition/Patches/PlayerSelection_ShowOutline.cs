using System;
using HarmonyLib;
using UnityEngine;

namespace Trudograd.NuclearEdition
{
    /// <summary>
    /// Changes the highlighting for locked doors and chests.
    /// </summary>
    [HarmonyPatch(typeof(PlayerSelection))]
    [HarmonyPatch("ShowOutline")]
    internal sealed class PlayerSelection_ShowOutline
    {
        static PlayerSelection_ShowOutline()
        {
            Debug.Log($"{nameof(NuclearEdition)} Apply patch: {nameof(PlayerSelection_ShowOutline)}");
        }
	    
        public static void Prefix(PlayerSelection __instance, EntityComponent entity, ref PlayerSelection.SelectionType type, Single intensity = 0f)
        {
            const PlayerSelection.SelectionType VioletColor = unchecked((PlayerSelection.SelectionType) 0xFF__EA_04_FF);

            if (type != PlayerSelection.SelectionType.Select)
                return;

            if (entity is ChestComponent chestComponent)
            {
                Chest chest = chestComponent.chest;
                if (chest.lockLevel != 0)
                {
                    type = VioletColor;
                    return;
                }
            }
            else if (entity is DoorComponent doorComponent)
            {
                var door = doorComponent.door;
                if (door.lockLevel != 0)
                {
                    type = VioletColor;
                    return;
                }
            }
        }
    }
}