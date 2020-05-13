using System;
using UnityEngine;

namespace Trudograd.NuclearEdition
{
    /// <summary>
    /// Implements the logic for automatically opening locked objects like doors and chests.
    /// </summary>
    internal static class LockerComponentOpener
    {
        public static Boolean Open(LockerComponent lockerComponent, ItemProto lockerKey, CharacterComponent character)
        {
            if (!InputManager.GetKey(InputManager.Action.Highlight))
                return HarmonyPrefixResult.CallOriginal;

            if (!lockerComponent.gameObject.activeInHierarchy)
                return HarmonyPrefixResult.CallOriginal;

            if (character.Character.Hallucinating.Level != ConditionLevel.Normal)
                return HarmonyPrefixResult.CallOriginal;

            if (lockerComponent.locker.lockLevel == 0)
                return HarmonyPrefixResult.CallOriginal;

            Item keyItem = null;
            if (lockerKey != null)
            {
                keyItem = (Item) lockerKey.CreateEntity();
                if (character.Character.HasItem(keyItem))
                    return HarmonyPrefixResult.CallOriginal;
            }

            Int32 maxLockpick = character.Character.Stats.Lockpick;
            
            foreach (CharacterComponent teammate in Game.World.GetAllTeamMates())
            {
                if (keyItem != null && teammate.Character.HasItem(keyItem))
                {
                    lockerComponent.Use(teammate);
                    return HarmonyPrefixResult.SkipOriginal;
                }

                Int32 teammateLockpick = teammate.Character.Stats.Lockpick;
                if (teammate.IsHuman() && teammate.Character.Hallucinating.Level == ConditionLevel.Normal && maxLockpick < teammateLockpick)
                {
                    character = teammate;
                    maxLockpick = teammateLockpick;
                }
            }

            character.Lockpick(lockerComponent);
            return HarmonyPrefixResult.SkipOriginal;
        }
    }
}