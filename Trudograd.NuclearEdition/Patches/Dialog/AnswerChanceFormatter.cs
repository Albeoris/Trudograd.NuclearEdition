using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Trudograd.NuclearEdition.Dialog
{
    public static class AnswerChanceFormatter
    {
        const String ColorTagBegin = "<b><color=#ffffffff>[";
        const String ColorTagEnd = "]</color></b>";

        public static void TryReplaceText(StateNode stateNode, ref String result)
        {
            DialogChanceRepresentation mode = Configuration.Dialog.DisplayChanceSuccess;
            if (mode == DialogChanceRepresentation.DoNotDisplay)
                return;

            var skillNode = GetSkillNodes(stateNode).FirstOrDefault();
            if (skillNode is null)
                return;

            String ownerId = $"{stateNode.GetType().FullName}: {stateNode.gameObject.GetInstanceID()}";
            
            String text = GameUtils.ToStyleText(result);
            Int32 colorIndexBegin = text.IndexOf(ColorTagBegin, StringComparison.Ordinal);
            if (colorIndexBegin < 0)
            {
                Debug.LogWarning($"[{nameof(NuclearEdition)}] {nameof(EncounterAnswerNode_Prepare)}: Failed to process answer: {text}");
                return;
            }
            
            var sb = new StringBuilder(text);

            Color color;
            String suffix = ColorTagEnd;

            if (mode == DialogChanceRepresentation.Value)
                FormatValue(skillNode, out color, out suffix);
            else
                FormatChance(ownerId, skillNode, mode, out color, ref suffix);

            // Replace color
            Byte r = (Byte) (color.r * 255);
            Byte g = (Byte) (color.g * 255);
            Byte b = (Byte) (color.b * 255);
            Byte a = (Byte) (color.a * 255);
            String colorText = $"<b><color=#{r:x2}{g:x2}{b:x2}{a:x2}>[";
            sb.Replace(ColorTagBegin, colorText, colorIndexBegin, ColorTagBegin.Length);

            // Add sufix
            if (suffix != ColorTagEnd)
            {
                Int32 endColorIndex = text.IndexOf(ColorTagEnd, colorIndexBegin, StringComparison.Ordinal);
                if (endColorIndex < 0)
                {
                    Debug.LogWarning($"[{nameof(NuclearEdition)}] {nameof(EncounterAnswerNode_Prepare)}: Failed to process answer: {text}");
                    return;
                }

                sb.Replace(ColorTagEnd, suffix, endColorIndex, ColorTagEnd.Length);
            }

            result = sb.ToString();
        }

        private static IEnumerable<HasSkillNode> GetSkillNodes(StateNode stateNode)
        {
            foreach (var node in stateNode.SubStates)
            {
                if (node is HasSkillNode skillNode)
                    yield return skillNode;
         
                else if (node is DummyNode dummyNode)
                {
                    foreach (var child in GetSkillNodes(dummyNode))
                        yield return child;
                }
            }
        }

        private static void FormatValue(HasSkillNode hasSkillNode, out Color color, out String suffix)
        {
            Single playerValue = Game.World.Player.CharacterComponent.Character.Stats.GetSkill(hasSkillNode.skill);
            Int32 requiredValue = hasSkillNode.value;
            color = playerValue >= requiredValue
                ? Color.green
                : Color.red;

            suffix = $" {playerValue} / {requiredValue}{ColorTagEnd}";
        }

        private static void FormatChance(String ownerId, HasSkillNode hasSkillNode, DialogChanceRepresentation displayChanceMode, out Color color, ref String suffix)
        {
            Single playerValue = Game.World.Player.CharacterComponent.Character.Stats.GetSkill(hasSkillNode.skill);
            Int32 requiredValue = hasSkillNode.value;
            Int32 inaccuracy = Configuration.Dialog.DisplayChanceAbsoluteInaccuracy + Configuration.Dialog.DisplayChanceRelativeInaccuracy * requiredValue / 100;

            // Randomize
            requiredValue += PersistentRandom.Get(ownerId, -inaccuracy / 2, inaccuracy / 2 + 1);

            Int32 minValue = requiredValue - inaccuracy;
            Int32 maxValue = requiredValue + inaccuracy;

            Single probability = Mathf.Clamp((playerValue - minValue) / (maxValue - minValue), 0, 1);

            color = probability < 0.5
                ? new Color(1.0f, probability / 0.5f, 0.0f)
                : new Color(1.0f - (probability - 0.5f) / 0.75f, 1.0f, 0.0f);
            
            if (displayChanceMode == DialogChanceRepresentation.Percent)
            {
                Int32 percent = (Int32) (probability * 100);
                suffix = $" {percent}%{ColorTagEnd}";
            }
        }
    }
}