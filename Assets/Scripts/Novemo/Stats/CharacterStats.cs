using System;
using System.Collections.Generic;
using Novemo.Status_Effects.Buffs;
using Novemo.Status_Effects.Debuffs;
using UnityEngine;

namespace Novemo.Stats
{
    public class CharacterStats : MonoBehaviour
    {
        //Character Level and Rarity
        public int level;
        public int stars;

        //Character Stats
        public List<Stat> stats = new List<Stat>();

        //Character status effects
        public List<Debuff> remainingDebuffs = new List<Debuff>();
        public List<Debuff> debuffs = new List<Debuff>();
        public List<Buff> remainingBuffs = new List<Buff>();
        public List<Buff> buffs = new List<Buff>();
        
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
        public event Action<float> OnDamageTake;
        public event Action<float, float> OnHealthChanged;
        public event Action<float, float> OnManaChanged;
        public event Action<float, float> OnExperienceChanged;

        public float GetLastDamage { get; set; }

        private void Awake()
        {
            CurrentHealth = stats[0].GetValue();
            CurrentMana = stats[1].GetValue();
            RequiredExperience = 50;
        }

        private void Update()
        {
            LevelUp();
            HandleDebuffs();
            HandleBuffs();

            if (DamageReducePercentage > 75f) {
                DamageReducePercentage = 75f;
            } if (DamageBoostPercentage > 125f) {
                DamageBoostPercentage = 125f;
            }
            
            if (Input.GetKeyDown(KeyCode.T))
            {
                TakeDamage(1, 1, 1, 1, false);
                TakeLethalDamage(1, 1);
            }

            if (Input.GetKey(KeyCode.X))
            {
                CurrentExperience += 5;
                OnExperienceChanged?.Invoke(RequiredExperience, CurrentExperience);
            }
            
            RegenerateHealth(stats[7].GetValue(), stats[28].GetValue());
            RegenerateMana(stats[8].GetValue(), stats[29].GetValue());

            if (CurrentHealth > stats[0].GetValue()) {
                CurrentHealth = stats[0].GetValue();
            } if (CurrentMana > stats[1].GetValue()) {
                CurrentMana = stats[1].GetValue();
            }

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        public void TakeDamage(float physicalDamage, float magicDamage, float physicalDamagePenetration, float magicDamagePenetration, bool isCrit)
        {
            //TODO Modify this formula (according to other effects, potions, scrolls mostly in %)
            physicalDamage -= physicalDamage * ((stats[3].GetValue() - physicalDamagePenetration) / (stats[3].GetValue() + physicalDamagePenetration)) * 0.75f;
            physicalDamage = Mathf.Clamp(physicalDamage, 0, float.MaxValue);
            physicalDamage = (float) Math.Round(physicalDamage * 100f) / 100f;

            magicDamage -= magicDamage * ((stats[4].GetValue() - magicDamagePenetration) / (stats[4].GetValue() + magicDamagePenetration)) * 0.75f;
            magicDamage = Mathf.Clamp(magicDamage, 0, float.MaxValue);
            magicDamage = (float) Math.Round(magicDamage * 100f) / 100f;

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

            OnDamageTake?.Invoke(damage);
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
        
        #region DebuffHandler
        
        public void ApplyDebuff(Debuff debuff)
        {
            if (debuffs.Contains(debuff))
            {
                remainingDebuffs.Add(debuff);
            }
            else
            {
                debuff.ApplyDebuff();
            }
        }

        public void RemoveDebuff(Debuff debuff)
        {
            debuffs.Remove(debuff);
        }

        private void HandleDebuffs()
        {
            if (debuffs.Count >= 1)
            {
                for (var i = 0; i < debuffs.Count; i++)
                {
                    debuffs[i].Update();
                }
            }

            if (remainingDebuffs.Count < 1) return;
            for (var i = 0; i < remainingDebuffs.Count; i++)
            {
                ApplyDebuff(remainingDebuffs[i]);

                remainingDebuffs.Remove(remainingDebuffs[i]);
            }
        }
        
        #endregion
        
        #region BuffHandler
        
        public void ApplyBuff(Buff buff)
        {
            if (buffs.Contains(buff))
            {
                remainingBuffs.Add(buff);
            }
            else
            {
                buff.ApplyBuff();
            }
        }

        public void RemoveBuff(Buff buff)
        {
            buffs.Remove(buff);
        }

        private void HandleBuffs()
        {
            if (buffs.Count >= 1)
            {
                for (var i = 0; i < buffs.Count; i++)
                {
                    buffs[i].Update();
                }
            }

            if (remainingBuffs.Count < 1) return;
            for (var i = 0; i < remainingBuffs.Count; i++)
            {
                ApplyBuff(remainingBuffs[i]);

                remainingBuffs.Remove(remainingBuffs[i]);
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
