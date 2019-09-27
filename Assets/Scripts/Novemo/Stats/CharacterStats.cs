using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Novemo.Stats
{
    public class CharacterStats : MonoBehaviour
    {
        // Character Level and Rarity
        public int level;
        public int stars;

        //Player Stats
        public List<Stat> stats = new List<Stat>();
        public Dictionary<string, float> scaleValues = new Dictionary<string, float>();

        //Health
        public float CurrentHealth { get; set; }
        private bool IsRegenHealth { get; set; }
        
        //Mana
        public float CurrentMana { get; set; }
        private bool IsRegenMana { get; set; }
        
        //Experience
        public float CurrentExperience { get; set; }
        public float RequiredExperience { get; set; } 
        public float experienceMultiplier;

        //Events
        public event Action<float, float> OnHealthChanged;
        public event Action<float, float> OnManaChanged;
        public event Action<float, float> OnExperienceChanged;
        
        public float GetLastDamage { get; set; }

        void Awake()
        {
            CurrentHealth = stats[0].GetValue();
            CurrentMana = stats[1].GetValue();
            RequiredExperience = 50;
        }

        void Update()
        {
            //Set movement speed
            
            LevelUp();
            
            if (Input.GetKeyDown(KeyCode.T))
            {
                TakeDamage(1, 1);
                TakeLethalDamage(1, 1);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                CurrentExperience += 5;
                OnExperienceChanged?.Invoke(RequiredExperience, CurrentExperience);
            }

            if (CurrentHealth < stats[0].GetValue() && !IsRegenHealth)
                StartCoroutine(RegenHealth(stats[7].GetValue(), stats[28].GetValue()));

            if (CurrentMana < stats[1].GetValue() && !IsRegenMana)
                StartCoroutine(RegenMana(stats[8].GetValue(), stats[29].GetValue()));

            if (CurrentHealth > stats[0].GetValue())
                CurrentHealth = stats[0].GetValue();

            if (CurrentMana > stats[1].GetValue())
                CurrentMana = stats[1].GetValue();

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        public void TakeDamage(float physicalDamage, float magicDamage)
        {
            //TODO Modify this formula (according to other effects, potions, scrolls mostly in %)
            physicalDamage -= physicalDamage * ((stats[3].GetValue() - stats[23].GetValue()) / (100 + stats[3].GetValue()));
            physicalDamage = Mathf.Clamp(physicalDamage, 0, float.MaxValue);
            physicalDamage = (float) Math.Round(physicalDamage * 100f) / 100f;

            magicDamage -= magicDamage * ((stats[4].GetValue() - stats[24].GetValue()) / (100 + stats[4].GetValue()));
            magicDamage = Mathf.Clamp(magicDamage, 0, float.MaxValue);
            magicDamage = (float) Math.Round(magicDamage * 100f) / 100f;

            var damage = physicalDamage + magicDamage;
            GetLastDamage = damage;

            CurrentHealth -= damage;

            OnHealthChanged?.Invoke(stats[0].GetValue(), CurrentHealth);
            
            Debug.Log(transform.name + " takes " + damage + " damage.");
        }

        public void TakeLethalDamage(float lethalPhysicalDamage, float lethalMagicDamage)
        {
            var damage = lethalPhysicalDamage + lethalMagicDamage;
            
            CurrentHealth -= damage;

            OnHealthChanged?.Invoke(stats[0].GetValue(), CurrentHealth);
            
            Debug.Log(transform.name + " takes " + damage + " damage.");
        }

        protected virtual void Die()
        {
            // Die in some way
            // Other either for player and enemy
        }

        protected virtual void LevelUp()
        {
            OnExperienceChanged?.Invoke(RequiredExperience, CurrentExperience);
        }
        
        #region Invokes
        
        public void OnHealthChangeInvoke() { OnHealthChanged?.Invoke(stats[0].GetValue(), CurrentHealth); }

        public void OnManaChangeInvoke() { OnManaChanged?.Invoke(stats[1].GetValue(), CurrentMana); }
        
        public void ExperienceInvoke() { OnExperienceChanged?.Invoke(RequiredExperience, CurrentExperience); }
        
        #endregion

        #region StatsScaling
        
        public float Scale(int statIndex, float modifier)
        {
            return stats[statIndex].GetValue() * modifier;
        }
        
        public IEnumerator ScaleValues(string modifierName, int statIndex, float modifierMultiplier)
        {
            stats[0].modifiers[modifierName] -= scaleValues[modifierName];
            scaleValues[modifierName] = Scale(statIndex, modifierMultiplier);
            stats[0].modifiers[modifierName] += scaleValues[modifierName];
            yield return new WaitForSeconds(1f);
        }
        
        #endregion

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
