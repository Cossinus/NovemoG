using System;
using System.Collections;
using System.Collections.Generic;
using Novemo.Classes;
using UnityEngine;

namespace Novemo.Stats
{
    public class CharacterStats : MonoBehaviour
    {
        // Character Level and Rarity
        public int level;
        public int stars;

        public float experienceModifier;

        //Player Stats
        public List<Stat> stats = new List<Stat>();
        
        public Dictionary<string, float> scaleValues = new Dictionary<string, float>();

        public float CurrentHealth { get; set; }
        public float CurrentMana { get; set; }
        public float CurrentExperience { get; set; }
        public float RequiredExperience { get; set; }
        public bool IsRegenHealth { get; set; }
        public bool IsRegenMana { get; set; }

        public event Action<float, float> OnHealthChanged;
        public event Action<float, float> OnManaChanged;

        void Awake()
        {
            CurrentHealth = stats[0].GetValue();
            CurrentMana = stats[1].GetValue();
            RequiredExperience = 50;
        }

        void Update()
        {
            //Set movement speed

            if (Math.Abs(CurrentHealth - stats[0].GetValue()) < stats[0].GetValue() && !IsRegenHealth)
                StartCoroutine(RegenHealth(stats[7].GetValue(), stats[28].GetValue()));

            if (Math.Abs(CurrentMana - stats[1].GetValue()) < stats[1].GetValue() && !IsRegenMana)
                StartCoroutine(RegenMana(stats[8].GetValue(), stats[29].GetValue()));

            if (CurrentExperience >= RequiredExperience)
            {
                float moveToNext = CurrentExperience - RequiredExperience;
                ClassManager.Instance.LevelUp();
                CurrentExperience += moveToNext;
            }
            
            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        public void TakeDamage(float physicalDamage, float magicDamage, float lethalPhysicalDamage,
            float lethalMagicDamage)
        {
            //TODO Modify this formula (according to other effects, potions, scrolls mostly in %)
            physicalDamage -= physicalDamage * ((stats[3].GetValue() - stats[23].GetValue()) / (100 + stats[3].GetValue()));
            physicalDamage = Mathf.Clamp(physicalDamage, 0, float.MaxValue);
            physicalDamage = (float) Math.Round(physicalDamage * 100f) / 100f;

            magicDamage -= magicDamage * ((stats[4].GetValue() - stats[24].GetValue()) / (100 + stats[4].GetValue()));
            magicDamage = Mathf.Clamp(magicDamage, 0, float.MaxValue);
            magicDamage = (float) Math.Round(magicDamage * 100f) / 100f;

            float damage = physicalDamage + magicDamage + lethalPhysicalDamage + lethalMagicDamage;

            CurrentHealth -= damage;

            Debug.Log(transform.name + " takes " + damage + " damage.");

            OnHealthChanged?.Invoke(stats[0].GetValue(), CurrentHealth);
        }

        public virtual void Die()
        {
            // Die in some way
            // Other either for player and enemy
        }

        public float Scale(int statIndex, float modifier)
        {
            return stats[statIndex].GetValue() * modifier;
        }
        
        public IEnumerator ScaleValues(string modifierName)
        {
            yield return new WaitForSeconds(1f);
            stats[0].modifiers[modifierName] -= scaleValues[modifierName];
            scaleValues[modifierName] = Scale(0, .03f);
            stats[0].modifiers[modifierName] += scaleValues[modifierName];
        }

        #region StatsRegen

        IEnumerator RegenHealth(float regenValue, float healthRegenRate)
        {
            IsRegenHealth = true;
            while (CurrentHealth < stats[0].GetValue())
            {
                CurrentHealth += regenValue;
                OnHealthChanged?.Invoke(stats[0].GetValue(), CurrentHealth);
                yield return new WaitForSeconds(healthRegenRate);
            }
            
            IsRegenHealth = false;
        }

        IEnumerator RegenMana(float regenValue, float manaRegenRate)
        {
            IsRegenMana = true;
            while (CurrentMana < stats[1].GetValue())
            {
                CurrentMana += regenValue;
                OnManaChanged?.Invoke(stats[1].GetValue(), CurrentMana);
                yield return new WaitForSeconds(manaRegenRate);
            }

            IsRegenMana = false;
        }

        #endregion
    }
}
