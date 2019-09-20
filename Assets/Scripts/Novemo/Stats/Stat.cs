using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Novemo.Stats
{
    [Serializable]
    public class Stat
    {
        public string statName;
        public float baseValue; 
        public ConcurrentDictionary<string, float> modifiers = new ConcurrentDictionary<string, float>();

        private float _tmpAS;
        
        public float GetValue()
        {
            var finalValue = baseValue;
            Parallel.ForEach(modifiers, x => finalValue += x.Value);
            return finalValue;
        }

        public void AddModifier(string name, float modifier)
        {
            if (modifier > 0)
            {
                if (modifiers.ContainsKey(name))
                {
                    if (name == "Attack Speed")
                    {
                        _tmpAS += modifier;
                        modifiers[name] = (100 + _tmpAS) / 100 * baseValue - baseValue;
                    }
                    else
                    {
                        modifiers[name] += modifier;
                    }
                }
                else
                {
                    if (name == "Attack Speed")
                    {
                        _tmpAS += modifier;
                        modifiers[name] = (100 + _tmpAS) / 100 * baseValue - baseValue;
                    }
                    else
                    {
                        modifiers[name] = modifier;
                    }
                }
            }
        }
    
        public void RemoveModifier(string name, float modifier)
        {
            if (modifier > 0)
            {
                if (name == "Attack Speed")
                {
                    _tmpAS -= modifier;
                    modifiers[name] = (100 + _tmpAS) / 100 * baseValue - baseValue;
                }
                else
                {
                    modifiers[name] -= modifier;
                }
            }
        }
    }
}