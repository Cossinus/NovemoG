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
        protected ConcurrentDictionary<string, float> modifiers = new ConcurrentDictionary<string, float>();

        [NonSerialized] public float modifierValue;
        [NonSerialized] public float tmpAttackSpeed;
        
        public float GetValue()
        {
            var finalValue = baseValue;
            Parallel.ForEach(modifiers, x => finalValue += x.Value);
            return finalValue;
        }
        
        // Clearly working for now
        public void Scale()
        {
            if (modifiers.ContainsKey("Scale"))
            {
                if (modifiers["Scale"] > 0)
                    modifiers["Scale"] = modifierValue * (GetValue() - modifiers["Scale"]);
                else
                    modifiers["Scale"] = modifierValue * GetValue();
            }
            else
            {
                modifiers["Scale"] = modifierValue * GetValue();
            }

            if (statName == "Health") {
                PlayerManager.Instance.player.GetComponent<CharacterStats>().CurrentHealth += modifiers["Scale"];
            } if (statName == "Mana") {
                PlayerManager.Instance.player.GetComponent<CharacterStats>().CurrentMana += modifiers["Scale"];
            }
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