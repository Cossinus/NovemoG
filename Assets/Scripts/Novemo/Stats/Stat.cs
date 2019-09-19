using System;
using System.Collections.Generic;

namespace Novemo.Stats
{
    [Serializable]
    public class Stat
    {
        public string statName;
        public float baseValue; 
        public Dictionary<string, float> modifiers = new Dictionary<string, float>();
    
        public float GetValue()
        {
            var finalValue = baseValue;
            foreach (var modifier in modifiers)
            {
                finalValue += modifier.Value;
            }
            return finalValue;
        }

        public void AddModifier(string name, float modifier)
        {
            if (modifier > 1)
            {
                if (modifiers.ContainsKey(name))
                    modifiers[name] += modifier;
                else
                    modifiers[name] = modifier;
            }
        }
    
        public void RemoveModifier(string name, float modifier)
        {
            if (modifier > 1)
            {
                modifiers[name] -= modifier;
            }
        }
    }
}