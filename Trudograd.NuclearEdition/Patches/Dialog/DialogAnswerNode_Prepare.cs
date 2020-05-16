using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace Trudograd.NuclearEdition.Dialog
{
    /// <summary>
    /// Adds a chance of success to the answer text.
    /// </summary>
    [HarmonyPatch(typeof(DialogAnswerNode), "Prepare")]
    internal sealed class DialogAnswerNode_Prepare
    {
        static DialogAnswerNode_Prepare()
        {
            Debug.Log($"[{nameof(NuclearEdition)}] Apply patch: {nameof(DialogAnswerNode_Prepare)}");
        }

        public static void Postfix(DialogAnswerNode __instance, ref String __result)
        {
            try
            {
                AnswerChanceFormatter.TryReplaceText(__instance, ref __result);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[{nameof(NuclearEdition)}] {nameof(DialogAnswerNode_Prepare)}: Failed to process answer: {__result}. Error: {ex}");
            }
        }
    }
}