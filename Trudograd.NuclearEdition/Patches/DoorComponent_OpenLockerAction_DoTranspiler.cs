using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace Trudograd.NuclearEdition
{
    /// <summary>
    /// Checks if the character has a key not only in the hand, but also in the inventory.
    /// </summary>
    [HarmonyPatch]
    internal sealed class DoorComponent_OpenLockerAction_DoTranspiler
    {
        static MethodBase TargetMethod()
        {
            return typeof(DoorComponent)
                .RequireNestedType("OpenLockerAction")
                .RequireSingleNestedType()
                .RequireInstanceMethod("<Do>b__0");
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var hasItem = typeof(Character).RequireInstanceMethod(nameof(Character.HasItem));
            var hasItemInHand = typeof(Character).RequireInstanceMethod(nameof(Character.HasItemInHand));
            var createEntity = typeof(EntityProto).RequireInstanceMethod(nameof(EntityProto.CreateEntity));

            Boolean patched = false;
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Callvirt && instruction.operand is MethodInfo target && target == hasItemInHand)
                {
                    yield return new CodeInstruction(OpCodes.Callvirt, createEntity);
                    yield return new CodeInstruction(OpCodes.Callvirt, hasItem);

                    patched = true;
                }
                else
                {
                    yield return instruction;
                }
            }

            Debug.Log($"{nameof(NuclearEdition)} Apply patch: {nameof(DoorComponent_OpenLockerAction_Do)}. Result: {patched}");
        }
    }
}