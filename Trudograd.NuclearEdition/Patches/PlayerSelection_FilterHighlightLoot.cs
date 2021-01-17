using System;
using System.Collections.Generic;
using ABBG;
using HarmonyLib;
using Trudograd.NuclearEdition.GameAPI;
using UnityEngine;

namespace Trudograd.NuclearEdition
{
    // get_CameraAlwaysSnap

    /// <summary>
    /// Turns off the highlighting of empty containers.
    /// </summary>
    [HarmonyPatch(typeof(PlayerSelection))]
    [HarmonyPatch(nameof(PlayerSelection.FilterHighlightLoot))]
    internal sealed class PlayerSelection_FilterHighlightLoot
    {
        static PlayerSelection_FilterHighlightLoot()
        {
            Debug.Log($"[{nameof(NuclearEdition)}] Apply patch: {nameof(PlayerSelection_FilterHighlightLoot)}");
        }

        public static void Postfix(PlayerSelection.Mode ____mode, EntityComponent entity, ref Boolean __result)
        {
            if (!__result)
                return;

            if (____mode != PlayerSelection.Mode.Scanner)
                return;

            if (IsFiltered(entity))
                __result = false;
        }

        private static Boolean IsFiltered(EntityComponent component)
        {
            // Filter empty containers
            if (Configuration.Scanner.DoNotHighlightEmptyContainers)
            {
                if (component is CharacterComponent characterComponent)
                    return CharacterComponentHelper.TryGetLootCost(characterComponent, out var cost) && cost == 0;

                if (component.Entity is Chest chest)
                    return chest.lockLevel == 0 && chest.Inventory.Count == 0;
            }

            // Filter standalone items by name
            if (component is ItemComponent itemComponent)
            {
                EntityProto prototype = itemComponent.Entity.Prototype;
                if (!String.IsNullOrEmpty(prototype.m_Localization))
                    return Configuration.Scanner.DoNotHighlightItems.Contains(prototype.Caption);
            }

            return false;
        }
    }
}