using System;
using System.Collections.Generic;
using Novemo.Status_Effects;
using UnityEngine;

namespace Novemo.Stats
{
    public class CharacterStats : MonoBehaviour
    {
        //Character Level
        public int level;

        //Character Stats
        public List<Stat> stats = new List<Stat>();
        
        //Metrics component
        protected Metrics metrics;

        //Character status effects
        public List<StatusEffect> statusEffects = new List<StatusEffect>();
        
        //Health
        public float CurrentHealth { get; set; }
        private float HealthRegenTimeElapsed { get; set; }
        
        //Mana
        public float CurrentMana { get; set; }
        private float ManaRegenTimeElapsed { get; set; }
        
        //Experience
        public float CurrentExperience { get; set; }
        public float RequiredExperience { get; set; } 
        public float experienceMultiplier;

        //Character additional damage boost/reduce
        public float DamageReducePercentage { get; set; }
        public float DamageBoostPercentage { get; set; }
        
        public bool CanAttack { get; set; } = true;

        //Events
        public event Action<float> OnDamageTook;
        public event Action<float, float> OnHealthChanged;
        public event Action<float, float> OnManaChanged;
        public event Action<float, float> OnExperienceChanged;

        public float GetLastDamage { get; set; }

        private void Awake()
        {
            CurrentHealth = stats[0].GetValue();
            CurrentMana = stats[1].GetValue();
            RequiredExperience = 50;
            metrics = GameObject.Find("GameManager").GetComponent<Metrics>();
        }

        private void Update()
        {
            if (CurrentHealth <= 0)
            {
                Die();
            }
            
            CurrentHealth = CurrentHealth > stats[0].GetValue() ? stats[0].GetValue() : CurrentHealth;
            CurrentMana = CurrentMana > stats[1].GetValue() ? stats[1].GetValue() : CurrentMana;

            RegenerateHealth(stats[7].GetValue(), stats[28].GetValue());
            RegenerateMana(stats[8].GetValue(), stats[29].GetValue());

            DamageBoostPercentage = DamageBoostPercentage > 125f ? 125f : DamageBoostPercentage;
            DamageReducePercentage = DamageReducePercentage > 75f ? 75f : DamageReducePercentage;
            
            LevelUp();
            HandleStatusEffects();

            if (Input.GetKey(KeyCode.T))
            {
                TakeDamage(1, 1, 1, 1, false);
                TakeLethalDamage(1, 1);
            }

            if (Input.GetKey(KeyCode.X))
            {
                CurrentExperience += 10;
                OnExperienceChanged?.Invoke(RequiredExperience, CurrentExperience);
                DamageBoostPercentage += 50f;
                DamageReducePercentage += 50f;
            }
        }

        public void TakeDamage(float physicalDamage, float magicDamage, float physicalDamagePenetration, float magicDamagePenetration, bool isCrit)
        {
            //TODO Modify this formula (according to other effects, potions, scrolls mostly in %)
            physicalDamage -= physicalDamage * ((stats[3].GetValue() - physicalDamagePenetration) / (stats[3].GetValue() + physicalDamagePenetration)) * 0.75f;
            physicalDamage = Mathf.Clamp(physicalDamage, 1, float.MaxValue);
            physicalDamage = (float) Math.Round(physicalDamage, 2);

            magicDamage -= magicDamage * ((stats[4].GetValue() - magicDamagePenetration) / (stats[4].GetValue() + magicDamagePenetration)) * 0.75f;
            magicDamage = Mathf.Clamp(magicDamage, 1, float.MaxValue);
            magicDamage = (float) Math.Round(magicDamage, 2);

            var damage = physicalDamage + magicDamage;
            
            if (DamageBoostPercentage > 0)
            {
                var tmpPercentage = 1 + DamageBoostPercentage / 100;
                damage *= tmpPercentage;
            }

            if (DamageReducePercentage > 0)
            {
                var tmpPercentage = 1 - DamageReducePercentage / 100;
                damage *= tmpPercentage;
            }
            
            if (isCrit)
            {
                damage *= 2;
            }

            OnDamageTook?.Invoke(damage);
            GetLastDamage = damage;

            CurrentHealth -= damage;
            OnHealthChanged?.Invoke(stats[0].GetValue(), CurrentHealth);
            
            Debug.Log($"{transform.name} takes {damage} damage.");
        }

        public void TakeLethalDamage(float lethalPhysicalDamage, float lethalMagicDamage)
        {
            var damage = lethalPhysicalDamage + lethalMagicDamage;
            
            CurrentHealth -= damage;

            OnHealthChanged?.Invoke(stats[0].GetValue(), CurrentHealth);
            
            Debug.Log($"{transform.name} takes {damage} damage.");
        }

        public void SetCurrentStat(int statIndex, float fraction)
        {
            CurrentHealth = stats[statIndex].GetValue() * fraction;
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

        public float GetScaledValueByMultiplier(int statIndex, float modifierMultiplier)
        {
            return stats[statIndex].GetValue() * modifierMultiplier;
        }

        #region StatusEffectHandler
        
        public void ApplyStatusEffect(StatusEffect statusEffect)
        {
            if (statusEffects.Contains(statusEffect))
            {
                statusEffects.Find(x => x.Equals(statusEffect)).RemoveEffect();
                statusEffect.ApplyEffect();
            }
            else
            {
                statusEffect.ApplyEffect();
            }
        }

        public void RemoveStatusEffect(StatusEffect statusEffect)
        {
            statusEffects.Remove(statusEffect);
        }

        private void HandleStatusEffects()
        {
            if (statusEffects.Count >= 1)
            {
                for (var i = 0; i < statusEffects.Count; i++)
                {
                    statusEffects[i].UpdateEffect();
                }
            }
        }
        
        #endregion
        
        #region Invokes
        
        public void OnHealthChangeInvoke() { OnHealthChanged?.Invoke(stats[0].GetValue(), CurrentHealth); }

        public void OnManaChangeInvoke() { OnManaChanged?.Invoke(stats[1].GetValue(), CurrentMana); }
        
        public void ExperienceInvoke() { OnExperienceChanged?.Invoke(RequiredExperience, CurrentExperience); }
        
        #endregion

        #region StatsRegen
        
        private void RegenerateHealth(float regenValue, float regenRate)
        {
            HealthRegenTimeElapsed += Time.deltaTime;

            if (!(HealthRegenTimeElapsed > 1f / regenRate)) return;
            
            if (!(CurrentHealth < stats[0].GetValue())) return;
            
            CurrentHealth += regenValue;
            OnHealthChanged?.Invoke(stats[0].GetValue(), CurrentHealth);
            HealthRegenTimeElapsed = 0;
        }
        
        private void RegenerateMana(float regenValue, float regenRate)
        {
            ManaRegenTimeElapsed += Time.deltaTime;

            if (!(ManaRegenTimeElapsed > 1f / regenRate)) return;
            
            if (!(CurrentMana < stats[1].GetValue())) return;
            
            CurrentMana += regenValue;
            OnManaChanged?.Invoke(stats[1].GetValue(), CurrentMana);
            ManaRegenTimeElapsed = 0;
        }

        #endregion
    }
}
