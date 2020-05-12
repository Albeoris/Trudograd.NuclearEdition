using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Trudograd.NuclearEdition
{
    /// <summary>
    /// Creates a prototype of a nonexistent key before opening the door and destroys it after completing the action
    /// to simplify the replacement of the IL-code.
    /// </summary>
    [HarmonyPatch]
    internal sealed class DoorComponent_OpenLockerAction_Do
    {
        private const String FakeName = nameof(DoorComponent_OpenLockerAction_Do) + ".Fake";

        private static readonly Type Type = typeof(DoorComponent).RequireNestedType("OpenLockerAction");
        private static readonly FieldInfo TargetField = Type.RequireInstanceField("target");

        static MethodBase TargetMethod()
        {
            return Type.RequireInstanceMethod("Do");
        }

        public static void Prefix(object __instance)
        {
            DoorComponent component = (DoorComponent) TargetField.GetValue(__instance);
            if (component.door.lockLevel != 0 && component.door.Prototype.lockerKey == null)
            {
                ItemProto fake = ScriptableObject.CreateInstance<ItemProto>();
                fake.name = FakeName;
                component.door.Prototype.lockerKey = fake;
            }
        }
    }
}