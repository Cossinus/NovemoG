using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Novemo.Player;
using UnityEngine;

namespace Novemo.Stats
{
    [Serializable]
    public class Stat
    {
        public string statName;
        public float baseValue;
        public ConcurrentDictionary<string, float> modifiers = new ConcurrentDictionary<string, float>();

        [NonSerialized] public float baseModifierValue;
        [NonSerialized] public float bonusModifierValue;
        [NonSerialized] public float wholeModifierValue;
        [NonSerialized] public float tmpAttackSpeed;
        
        public float GetValue()
        {
            var finalValue = baseValue;
            Parallel.ForEach(modifiers, x => finalValue += x.Value);
            return finalValue;
        }

        public void AddBaseValue(float value)
        {
            baseValue += value;
            Scale();
        }
        
        // Clearly working for now
        public void Scale()
        {
            Metrics.CalculateScale(this, "Whole", wholeModifierValue);
            Metrics.CalculateScale(this, "Bonus", bonusModifierValue);
            Metrics.CalculateScale(this, "Base", baseModifierValue);
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
                }
                else
                {
                    modifiers[name] += modifier;
                    Scale();
                }
            }
            else
            {
                if (name == "Attack Speed")
                {
                    tmpAttackSpeed += modifier;
                    modifiers[name] = (100 + tmpAttackSpeed) / 100 * baseValue - baseValue;
                }
                else
                {
                    modifiers[name] = modifier;
                    Scale();
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
                Scale();
            }
        }
    }
}