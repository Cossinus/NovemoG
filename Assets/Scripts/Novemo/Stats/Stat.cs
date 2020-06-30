using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Novemo.Characters.Player;
using Novemo.Inventories;
using UnityEngine;

namespace Novemo.Stats
{
    [Serializable]
    public class Stat
    {
        public string statName;
        public float baseValue;
        public ConcurrentDictionary<string, float> modifiers = new ConcurrentDictionary<string, float>();

        [HideInInspector] public float baseModifierValue;
        [HideInInspector] public float bonusModifierValue;
        [HideInInspector] public float wholeModifierValue;
        [HideInInspector] public float tmpAttackSpeed;

        public float GetValue()
        {
            var finalValue = baseValue;
            Parallel.ForEach(modifiers, x => finalValue += x.Value);
            return finalValue;
        }

        public float GetBonusValue()
        {
            float finalValue = 0;
            Parallel.ForEach(modifiers, x => finalValue += x.Value);
            return finalValue;
        }

        public void AddBaseValue(float value)
        {
            baseValue += value;
            ScaleAll();
        }

        public void AddWholeModifier(float value, Characters.Character target)
        {
            wholeModifierValue += value;
            CalculateScale(this, "Whole", wholeModifierValue, target);
        }

        public void AddBonusModifier(float value, Characters.Character target)
        {
            bonusModifierValue += value;
            CalculateScale(this, "Bonus", bonusModifierValue, target);
        }
        
        public void AddBaseModifier(float value, Characters.Character target)
        {
            baseModifierValue += value;
            CalculateScale(this, "Base", baseModifierValue, target);
        }

        public void ScaleAll()
        {
            CalculateScale(this, "Whole", wholeModifierValue, PlayerManager.Instance.player.GetComponent<Player>());
            CalculateScale(this, "Bonus", bonusModifierValue, PlayerManager.Instance.player.GetComponent<Player>());
            CalculateScale(this, "Base", baseModifierValue, PlayerManager.Instance.player.GetComponent<Player>());
        }

        public void AddModifier(string name, float modifier)
        {
            if (!(modifier > 0)) return;
            if (modifiers.ContainsKey(name))
            {
                if (name == "Attack Speed")
                {
                    tmpAttackSpeed += modifier;
                    modifiers[name] = (100 + tmpAttackSpeed) / 100 * baseValue - baseValue;
                    StatsPanel.Instance.UpdateStatsText();
                }
                else
                {
                    modifiers[name] += modifier;
                    ScaleAll();
                }
            }
            else
            {
                if (name == "Attack Speed")
                {
                    tmpAttackSpeed += modifier;
                    modifiers[name] = (100 + tmpAttackSpeed) / 100 * baseValue - baseValue;
                    StatsPanel.Instance.UpdateStatsText();
                }
                else
                {
                    modifiers[name] = modifier;
                    ScaleAll();
                }
            }
        }
    
        public void RemoveModifier(string name, float modifier)
        {
            if (!(modifier > 0)) return;
            if (name == "Attack Speed")
            {
                tmpAttackSpeed -= modifier;
                modifiers[name] = (100 + tmpAttackSpeed) / 100 * baseValue - baseValue;
            }
            else
            {
                modifiers[name] -= modifier;
                ScaleAll();
            }
        }
        
        private static void CalculateScale(Stat stat, string scaleType, float modifierValue, Characters.Character target)
        {
            var healthFraction = Metrics.GetCurrentFraction(true, target);

            stat.modifiers[scaleType] = stat.modifiers.ContainsKey(scaleType)
                ? stat.modifiers[scaleType] > 0
                    ? modifierValue * (stat.GetValue() - stat.modifiers[scaleType])
                    : modifierValue * stat.GetValue()
                : modifierValue * stat.GetValue();
			
            target.SetCurrentStat(0, healthFraction);
            
            StatsPanel.Instance.UpdateStatsText();
        }
    }
}