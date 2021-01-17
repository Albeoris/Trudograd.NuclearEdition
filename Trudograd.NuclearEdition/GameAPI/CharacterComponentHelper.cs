using System;

namespace Trudograd.NuclearEdition.GameAPI
{
    public static class CharacterComponentHelper
    {
        public static Boolean TryGetLootCost(CharacterComponent characterComponent, out Int32 itemsCost)
        {
            if (!characterComponent.IsDead())
            {
                itemsCost = default;
                return false;
            }

            const String key = "Trudograd.NuclearEdition.PlayerSelection_ShowOutline.OnDeadLoot";

            if (!characterComponent.Character.HasKey(key))
            {
                characterComponent.Character.AddKey(key);

                Boolean fromCannibal = Game.World.Player.CharacterComponent.Character.CharProto.Stats.HasPerk(CharacterStats.Perk.Cannibal);
                characterComponent.OnDeadLoot(fromCannibal);
            }

            itemsCost = characterComponent.Character.GetItemsCost();
            return true;
        }
    }
}