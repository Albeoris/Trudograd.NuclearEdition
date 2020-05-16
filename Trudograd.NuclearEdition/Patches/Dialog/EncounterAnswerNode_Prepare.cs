using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;
using Object = System.Object;
using Random = System.Random;

namespace Trudograd.NuclearEdition.Dialog
{
    /// <summary>
    /// Adds a chance of success to the answer text.
    /// </summary>
    [HarmonyPatch(typeof(EncounterAnswerNode), "Prepare")]
    internal sealed class EncounterAnswerNode_Prepare
    {
        static EncounterAnswerNode_Prepare()
        {
            Debug.Log($"[{nameof(NuclearEdition)}] Apply patch: {nameof(EncounterAnswerNode_Prepare)}");
        }

        public static void Postfix(EncounterAnswerNode __instance, ref String __result)
        {
            try
            {
                AnswerChanceFormatter.TryReplaceText(__instance, ref __result);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[{nameof(NuclearEdition)}] {nameof(EncounterAnswerNode_Prepare)}: Failed to process answer: {__result}. Error: {ex}");
            }
        }
    }
}