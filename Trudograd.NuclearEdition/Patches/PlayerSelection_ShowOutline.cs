﻿using System;
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
            const PlayerSelection.SelectionType GrayColor = unchecked((PlayerSelection.SelectionType)0xFF____80_80_80);
            const PlayerSelection.SelectionType VioletColor = unchecked((PlayerSelection.SelectionType) 0xFF__EA_04_FF);

            if (type != PlayerSelection.SelectionType.Select)
                return;
            
            if (Game.World.Player.CharacterComponent.Character.Hallucinating.Level != ConditionLevel.Normal)
                return;
            
            if (entity is CharacterComponent characterComponent)
            {
                if (!characterComponent.IsDead())
                    return;

                const String key = "Trudograd.NuclearEdition.PlayerSelection_ShowOutline.OnDeadLoot";

                if (!characterComponent.Character.HasKey(key))
                {
                    characterComponent.Character.AddKey(key);

                    Boolean fromCannibal = Game.World.Player.CharacterComponent.Character.CharProto.Stats.HasPerk(CharacterStats.Perk.Cannibal);
                    characterComponent.OnDeadLoot(fromCannibal);
                }

                if (characterComponent.Character.GetItemsCost() == 0)
                    type = GrayColor;
            }
            else if (entity is ChestComponent chestComponent)
            {
                Chest chest = chestComponent.chest;
                if (chest.lockLevel != 0)
                {
                    type = VioletColor;
                    return;
                }

                if (chest.Inventory.Count == 0)
                    type = GrayColor;
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