using System;
using System.Collections.Generic;
using Novemo.Abilities;
using Novemo.Stats;
using Novemo.StatusEffects;
using Novemo.StatusEffects.Debuffs;
using Novemo.UI;
using UnityEngine;

namespace Novemo.Characters
{
    public class Character : MonoBehaviour
    {
        [Header("Character's Level")]
        public int level;

        #region Lists
        
        /// <summary>Container for character's stats.</summary>
        /// <para>Health - 0</para>
        /// <para>Mana - 1</para>
        /// <para>Damage = 2</para>
        /// <para>Armor - 3</para>
        /// <para>Magic Resist - 4</para>
        /// <para>Attack Speed - 5</para>
        /// <para>Movement Speed - 6</para>
        /// <para>Health Regen - 7</para>
        /// <para>Mana Regen - 8</para>
        /// <para>Arcane - 9</para>
        /// <para>Lethal Damage - 10</para>
        /// <para>Cooldown Reduction - 11</para>
        /// <para>+Gold - 12</para>
        /// <para>+Exp - 13</para>
        /// <para>Mana Burn Chance - 14</para>
        /// <para>Bleed Chance - 15</para>
        /// <para>Poison Chance - 16</para>
        /// <para>Withering Chance - 17</para>
        /// <para>Ignite Chance - 18</para>
        /// <para>Max Health Damage - 19</para>
        /// <para>Current Health Damage - 20</para>
        /// <para>Attack Pair Chance - 21</para>
        /// <para>Attack Block Chance - 22</para>
        /// <para>Armor Penetration - 23</para>
        /// <para>Magic Resist Penetration - 24</para>
        /// <para>Life Steal - 25</para>
        /// <para>Spell Vampirism - 26</para>
        /// <para>Luck - 27</para>
        /// <para>Health Regen Rate - 28</para>
        /// <para>Mana Regen Rate - 29</para>
        /// <para>Crit Chance - 30</para>
        [Header("Character's Stats")]
        public List<Stat> stats = new List<Stat>();

        /// <summary>
        /// A list of status effects that are applied on a character.
        /// </summary>
        public List<StatusEffect> statusEffects = new List<StatusEffect>();

        #endregion

        #region Properties
        
        public float CurrentHealth { get; protected set; }
        public float CurrentShield { get; protected set; }
        private float HealthRegenTimeElapsed { get; set; }
        
        public float CurrentMana { get; protected set; }
        private float ManaRegenTimeElapsed { get; set; }
        
        public float CurrentExperience { get; protected set; }
        public float RequiredExperience { get; set; }
        [Header("Experience Settings")]
        public float experienceMultiplier;
        
        private float _damageReducePercentage;
        /// <summary>
        /// Character's Damage Reduction expressed in percentage.
        /// </summary>
        public float DamageReducePercentage
        {
            get => _damageReducePercentage;
            set => _damageReducePercentage = _damageReducePercentage >= 75f ? 75f : value;
        }

        private float _damageBoostPercentage;
        /// <summary>
        /// Character's Damage Boost expressed in percentage.
        /// </summary>
        public float DamageBoostPercentage
        {
            get => _damageBoostPercentage;
            set => _damageBoostPercentage = _damageBoostPercentage >= 125f ? 125f : value;
        }

        public bool FullHP => Metrics.EqualFloats(CurrentHealth, stats[0].GetValue(), 0.01);

        public bool CanUseSpells { get; set; } = true;
        public bool CanAttack { get; set; } = true;
        public bool CanMove { get; set; } = true;

        public bool BadLuck { get; set; }
        
        public bool Stunned { get; set; }
        public bool Silenced { get; set; }
        public bool Invulnerable { get; set; }
        
        public bool Bleeding { get; set; }
        public bool Withering { get; set; }
        public bool Ignited { get; set; }
        public bool Poisoned { get; set; }

        //TODO Set this up in Attack function in charactercombat
        public DateTime LastDamageDone { get; set; }
        public float OutOfCombatTime { get; private set; }

        public TargetType TargetType { get; protected set; }

        #endregion
        
        #region Actions

        /// <summary>
        /// Gives information about an amount that Current Health was modified
        /// </summary>
        public event Action<float> OnHealthModified;
        /// <summary>
        /// Gives information about an amount that Current Shield was modified
        /// </summary>
        public event Action<float> OnShieldModified;
        /// <summary>
        /// Gives information about an amount that Current Mana was modified
        /// </summary>
        public event Action<float> OnManaModified;
        /// <summary>
        /// Gives information about an amount that Current Experience was modified
        /// </summary>
        public event Action<float> OnExperienceModified; 
        
        /// <summary>
        /// Gives information about damage taken by the character
        /// </summary>
        /// <para>1. Source</para>
        /// <para>2. Target</para>
        /// <para>The actual damage</para>
        public event Action<Character, Character, float> OnDamageTaken;
        /// <summary>
        /// Triggers character's death and passes all its values
        /// </summary>
        /// <para>1. Source</para>
        /// <para>2. Target</para>
        public event Action<Character, Character> OnCharacterDeath;
        
        /// <summary>
        /// Gives information about Current Health and Max Health
        /// </summary>
        public event Action<float, float, float> OnHealthChanged;
        /// <summary>
        /// Gives information about Current Mana and Max Mana
        /// </summary>
        public event Action<float, float> OnManaChanged;
        /// <summary>
        /// Gives information about Current Experience and Required Experience
        /// </summary>
        public event Action<float, float> OnExperienceChanged;

        #endregion

        private void Awake()
        {
            CurrentHealth = stats[0].GetValue();
            CurrentMana = stats[1].GetValue();
            RequiredExperience = 50;
        }

        private void Update()
        {
            RegenerateHealth(stats[7].GetValue(), stats[28].GetValue());
            RegenerateMana(stats[8].GetValue(), stats[29].GetValue());

            HandleStatusEffects();

            if (Input.GetKey(KeyCode.T))
            {
                TakeDamage(this, 1, 1);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                ModifyShield(10f);
                
                var statBuff = new Bleeding
                {
                    EffectDuration = 10f,
                    Icon = Resources.Load<Sprite>("Sprites/Effects/Bleeding"),
                    StatIndex = 0,
                    EffectName = "Bleeding",
                    IsDebuff = false,
                    EffectPower = 10f,
                    EffectRate = 1,
                    TargetStats = this,
                    SourceStats = this
                };
                ApplyStatusEffect(statBuff);
                
                var statDebuff = new Bleeding
                {
                    EffectDuration = 10f,
                    Icon = Resources.Load<Sprite>("Sprites/Effects/Bleeding"),
                    StatIndex = 0,
                    EffectName = "Bleeding2",
                    IsDebuff = true,
                    EffectPower = 15f,
                    EffectRate = 1,
                    TargetStats = this,
                    SourceStats = this
                };
                ApplyStatusEffect(statDebuff);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                AddExperience(10);
                DamageBoostPercentage += 50f;
                DamageReducePercentage += 50f;
            }

            if (LastDamageDone.AddSeconds(15) < DateTime.UtcNow)
            {
                OutOfCombatTime += Time.deltaTime;
            }
        }

        /// <summary>
        /// Deals damage to a character using raw passed values to calculate final damage. Reduced by armor/magic resist.
        /// </summary>
        /// <param name="source">Source Character reference</param>
        /// <param name="physicalDamage">Raw physical damage</param>
        /// <param name="magicDamage">Raw magical damage</param>
        public void TakeDamage(Character source, float physicalDamage, float magicDamage)
        {
            if (Invulnerable) return;

            var damage = Metrics.PhysicalDamage(source, this, physicalDamage,
                             stats[3].GetValue(), source.stats[23].GetValue()) +
                         Metrics.MagicDamage(source, this, magicDamage,
                             stats[4].GetValue(), source.stats[24].GetValue());

            DoDamage(source, damage);
        }

        /// <summary>
        /// Deals damage with passed values to a character. Ignores armor/magic resist and any debuff/buff excluding shields.
        /// </summary>
        /// <param name="source">Source Character reference</param>
        /// <param name="lethalPhysicalDamage">Raw lethal physical damage</param>
        /// <param name="lethalMagicDamage">Raw lethal magical damage</param>
        public void TakeLethalDamage(Character source, float lethalPhysicalDamage, float lethalMagicDamage)
        {
            if (Invulnerable) return;
            
            var damage = lethalPhysicalDamage + lethalMagicDamage;
            
            DoDamage(source, damage);
        }

        /// <summary>
        /// Deals actual damage to a character using passed calculated damage value.
        /// </summary>
        /// <param name="source">Source Character reference</param>
        /// <param name="damage">Calculated damage value</param>
        private void DoDamage(Character source, float damage)
        {
            damage = Mathf.Clamp(damage, 0, float.MaxValue);

            var shieldDamage = Math.Min(CurrentShield, damage);
            var healthDamage = Math.Min(CurrentHealth, damage - shieldDamage);

            CurrentShield -= shieldDamage;
            
            CurrentHealth -= healthDamage;
            
            if (CurrentHealth <= 0)
            {
                OnCharacterDeath?.Invoke(source, this);
            }
            
            OnHealthChangeInvoke();

            OnDamageTaken?.Invoke(source, this, damage);
            
            LastDamageDone = DateTime.UtcNow;
            
            Debug.Log($"{transform.name} takes {damage} damage.");
        }

        /// <summary>
        /// Adds or removes health from a character using passed value. Modified by buffs/debuffs and various character's status effects.
        /// </summary>
        /// <param name="amount">Desired amount to modify character's health</param>
        /// <returns>Amount of health to modify</returns>
        public void ModifyHealth(float amount)
        {
            if (Withering && amount > 0)
            {
                amount = 0;
            }
            else if ((Bleeding || Ignited) && amount > 0)
            {
                amount *= 0.5f;
            }
            else if (Poisoned && amount > 0)
            {
                amount *= 0.85f;
            }

            CurrentHealth += amount;

            CurrentHealth = CurrentHealth > stats[0].GetValue() ? stats[0].GetValue() : CurrentHealth;
            
            OnHealthModified?.Invoke(amount);
            OnHealthChangeInvoke();
        }

        /// <summary>
        /// Adds or removes shield from a character using passed value. Modified by Bleeding debuff.
        /// </summary>
        /// <param name="amount">Desired amount to modify character's health</param>
        /// <returns>Amount of shield to modify</returns>
        public void ModifyShield(float amount)
        {
            if (Bleeding && amount > 0)
            {
                amount *= 0.5f;
            }

            CurrentShield += amount;
            
            OnShieldModified?.Invoke(amount);
            OnHealthChangeInvoke();
        }

        /// <summary>
        /// Adds or removes mana from a character using passed value. Modified by buffs/debuffs and various character's status effects.
        /// </summary>
        /// <param name="amount">Desired amount to modify character's mana</param>
        /// <returns>Amount of mana to modify</returns>
        public void ModifyMana(float amount)
        {
            if (Withering && amount > 0)
            {
                amount *= 0.25f;
            }
            else if (Ignited && amount > 0)
            {
                amount *= 0.65f;
            }
            
            CurrentMana += amount;
            
            CurrentMana = CurrentMana > stats[1].GetValue() ? stats[1].GetValue() : CurrentMana;
            
            OnManaModified?.Invoke(amount);
            OnManaChangeInvoke();
        }

        public void AddExperience(float amount)
        {
            if (BadLuck)
            {
                amount *= 0.5f;
            }

            CurrentExperience += amount;

            if (CurrentExperience >= RequiredExperience) LevelUp();
            
            OnExperienceModified?.Invoke(amount);
            OnExperienceChangeInvoke();
        }

        /// <summary>
        /// Calculates how Character's current Health/Mana should change after adding or reducing max Health/Mana
        /// to potentially avoid fake healing with items/other effects. Should be used after modifying max
        /// Health/Mana values.
        /// </summary>
        /// <param name="statIndex">Health - 0 or Mana - 1</param>
        /// <param name="fraction">Current Health/Mana divided by Max Health/Mana calculated before modifying Max Health/Mana value</param>
        public void SetCurrentStat(int statIndex, float fraction)
        {
            if (statIndex == 0)
            {
                CurrentMana = stats[statIndex].GetValue() * fraction;
                OnManaChangeInvoke();
            }
            else
            {
                CurrentHealth = stats[statIndex].GetValue() * fraction;
                OnHealthChangeInvoke();
            }
        }

        /// <summary>
        /// Triggers character's level up.
        /// </summary>
        public virtual void LevelUp()
        {
            OnExperienceChanged?.Invoke(RequiredExperience, CurrentExperience);
        }

        /// <summary>
        /// Returns scaled stat value with given multiplier.
        /// </summary>
        /// <param name="statIndex">Index in a list of character's stats</param>
        /// <param name="modifierMultiplier">Multiplier used for calculation</param>
        public float GetScaledValueByMultiplier(int statIndex, float modifierMultiplier)
        {
            return stats[statIndex].GetValue() * modifierMultiplier;
        }

        #region StatusEffectHandler
        
        /// <summary>
        /// Applies a status effect to a character. Requires StatusEffect object with set all main values. If the same
        /// effect that character has is passed old effect will be removed and new one will be added.
        /// </summary>
        /// <param name="statusEffect">StatusEffect object with set all main values</param>
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

            if (statusEffect.IsPassive) return;
            var color = statusEffect.IsDebuff ? "<color=red>" : "<color=green>";
            EventLog.Instance.RaiseEventLog(
                $"{color}<b>You got {statusEffect.GetType().Name} for {statusEffect.EffectDuration} seconds!</b></color>");
        }

        /// <summary>
        /// Removes status effect from a character
        /// </summary>
        /// <param name="statusEffect">Desired status effect</param>
        public void RemoveStatusEffect(StatusEffect statusEffect)
        {
            statusEffects.Remove(statusEffect);
        }

        /// <summary>
        /// Removes a negative effect with passed name from a character
        /// </summary>
        /// <param name="effectName">Name of an effect to remove</param>
        public void RemoveEffectWithName(string effectName)
        {
            statusEffects.Find(x => x.EffectName.Contains(effectName)).RemoveEffect();
        }

        /// <summary>
        /// Removes all negative effects from a character
        /// </summary>
        public void RemoveNegativeEffects()
        {
            statusEffects.Find(x => x.EffectName.Contains("Stun")).RemoveEffect();
            statusEffects.Find(x => x.EffectName.Contains("Slow")).RemoveEffect();
            statusEffects.Find(x => x.EffectName.Contains("Silence")).RemoveEffect();
            statusEffects.Find(x => x.EffectName.Contains("Root")).RemoveEffect();
            statusEffects.Find(x => x.EffectName.Contains("Poison")).RemoveEffect();
            statusEffects.Find(x => x.EffectName.Contains("Withering")).RemoveEffect();
            statusEffects.Find(x => x.EffectName.Contains("Bleeding")).RemoveEffect();
        }

        /// <summary>
        /// Handles behavior of character's status effects
        /// </summary>
        private void HandleStatusEffects()
        {
            if (statusEffects.Count < 1) return;
            
            for (var i = 0; i < statusEffects.Count; i++)
            {
                statusEffects[i].UpdateEffect();
            }
        }
        
        #endregion
        
        #region Invokes
        
        /// <summary>
        /// Invokes OnHealthChanged action with current max health and current health. Must be used after modifying any
        /// of these values.
        /// </summary>
        public void OnHealthChangeInvoke() { OnHealthChanged?.Invoke(stats[0].GetValue(), CurrentHealth, CurrentShield); }

        /// <summary>
        /// Invokes OnManaChanged action with current max mana and current mana. Must be used after modifying any of
        /// these values.
        /// </summary>
        public void OnManaChangeInvoke() { OnManaChanged?.Invoke(stats[1].GetValue(), CurrentMana); }
        
        /// <summary>
        /// Invokes OnExperienceChanged action with current required experience and current experience. Must be used
        /// after modifying any of these values.
        /// </summary>
        public void OnExperienceChangeInvoke() { OnExperienceChanged?.Invoke(RequiredExperience, CurrentExperience); }
        
        #endregion

        #region StatsRegen
        
        /// <summary>
        /// Heals a character for a passed amount every given tick. Modified by buffs/debuffs and various character's status effects.
        /// </summary>
        /// <param name="regenValue"></param>
        /// <param name="regenRate"></param>
        private void RegenerateHealth(float regenValue, float regenRate)
        {
            HealthRegenTimeElapsed += Time.deltaTime;

            if (!(HealthRegenTimeElapsed > 1f / regenRate)) return;
            
            if (!(CurrentHealth < stats[0].GetValue())) return;

            ModifyHealth(regenValue);
            HealthRegenTimeElapsed = 0;
        }
        
        /// <summary>
        /// Regenerates character's mana by a passed amount every given tick. Modified by buffs/debuffs and various character's status effects.
        /// </summary>
        /// <param name="regenValue"></param>
        /// <param name="regenRate"></param>
        private void RegenerateMana(float regenValue, float regenRate)
        {
            ManaRegenTimeElapsed += Time.deltaTime;

            if (!(ManaRegenTimeElapsed > 1f / regenRate)) return;
            
            if (!(CurrentMana < stats[1].GetValue())) return;

            ModifyMana(regenValue);
            ManaRegenTimeElapsed = 0;
        }

        #endregion
    }
}
