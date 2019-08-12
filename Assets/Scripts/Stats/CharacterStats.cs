using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterStats : MonoBehaviour
{
    public int level;
    public int stars;
    public List<Stat> stats = new List<Stat>();
    public float CurrentHealth { get; set; }
    public float CurrentMana { get; set; }
    public float CurrentExperience { get; set; }
    public bool IsRegenHealth { get; set; }
    public bool IsRegenMana { get; set; }

    #region StatsAsNames
    
    private float MaxHealth;
    private float MaxMana;
    private float Damage;
    private float Armor;
    private float MagicResist;
    private float AttackSpeed;
    private float MovementSpeed;
    private float HealthRegen;
    private float ManaRegen;
    private float Arcane;
    private float LethalDamage;
    private float CooldownRedutcion;
    private float AdditionalGold;
    private float AdditionalExp;
    private float ManaBurnChance;
    private float BleedChance;
    private float PoisonChance;
    private float WitheringChance;
    private float IgniteChance;
    private float MaxHealthDamage;
    private float CurrentHealthDamage;
    private float AttackPairChance;
    private float AttackBlockChance;
    private float ArmorPenetration;
    private float MagicResistPenetration;
    private float LifeSteal;
    private float SpellVampirism;
    private float Luck;
    private float HealthRegenRate;
    private float ManaRegenRate;

    #endregion
    
    public event Action<float, float> OnHealthChanged;
    public event Action<float, float> OnManaChanged;
    
    void Awake()
    {
        SetStats();
        CurrentHealth = stats[0].GetValue();
        CurrentMana = stats[1].GetValue();
        GetComponent<NavMeshAgent>().speed = stats[6].GetValue();
    }

    void Update()
    {
        SetStats();
        
        if (CurrentHealth != MaxHealth && !IsRegenHealth)
            StartCoroutine(RegenHealth(HealthRegen, HealthRegenRate));
        
        if (CurrentMana != MaxMana && !IsRegenMana)
            StartCoroutine(RegenMana(ManaRegen, ManaRegenRate));
    }

    public void TakeDamage(float physicalDamage, float magicDamage, float lethalPhysicalDamage, float lethalMagicDamage)
    {
        // Swap this with own formula (according to other effects, potions, scrolls, armor mostly in %)
        physicalDamage -= physicalDamage * ((Armor - ArmorPenetration) / (100 + Armor));
        physicalDamage = Mathf.Clamp(physicalDamage, 0, float.MaxValue);
        physicalDamage = (float) Math.Round(physicalDamage * 100f) / 100f;
        
        magicDamage -= magicDamage * ((MagicResist - MagicResistPenetration) / (100 + MagicResist));
        magicDamage = Mathf.Clamp(magicDamage, 0, float.MaxValue);
        magicDamage = (float) Math.Round(magicDamage * 100f) / 100f;

        float damage = physicalDamage + magicDamage + lethalPhysicalDamage + lethalMagicDamage;
        
        CurrentHealth -= damage;
        Debug.Log(transform.name + " takes " + physicalDamage + " damage.");

        OnHealthChanged?.Invoke(MaxHealth, CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // Die in some way
        // Other either for player and enemy
    }

    IEnumerator RegenHealth(float regenValue, float healthRegenRate)
    {
        IsRegenHealth = true;
        while (CurrentHealth < MaxHealth)
        {
            CurrentHealth += regenValue;
            OnHealthChanged?.Invoke(MaxHealth, CurrentHealth);
            yield return new WaitForSeconds(healthRegenRate);
        }
        IsRegenHealth = false;
    }
    
    IEnumerator RegenMana(float regenValue, float manaRegenRate)
    {
        IsRegenMana = true;
        while (CurrentMana < MaxMana)
        {
            CurrentMana += regenValue;
            OnManaChanged?.Invoke(MaxMana, CurrentMana);
            yield return new WaitForSeconds(manaRegenRate);
        }
        IsRegenMana = false;
    }

    #region Assign properties
    
    private void SetStats()
    {
        foreach (var stat in stats)
        {
            if (stat.Name == "Health")
                MaxHealth = stat.GetValue();
            if (stat.Name == "Mana")
                MaxMana = stat.GetValue();
            if (stat.Name == "Damage")
                Damage = stat.GetValue();
            if (stat.Name == "Armor")
                Armor = stat.GetValue();
            if (stat.Name == "Magic Resist")
                MagicResist = stat.GetValue();
            if (stat.Name == "Attack Speed")
                AttackSpeed = stat.GetValue();
            if (stat.Name == "Movement Speed")
                MovementSpeed = stat.GetValue();
            if (stat.Name == "Health Regen")
                HealthRegen = stat.GetValue();
            if (stat.Name == "Mana Regen")
                ManaRegen = stat.GetValue();
            if (stat.Name == "Arcane")
                Arcane = stat.GetValue();
            if (stat.Name == "Lethal Damage")
                LethalDamage = stat.GetValue();
            if (stat.Name == "Cooldown Reduction")
                CooldownRedutcion = stat.GetValue();
            if (stat.Name == "+Gold")
                AdditionalGold = stat.GetValue();
            if (stat.Name == "+Exp")
                AdditionalExp = stat.GetValue();
            if (stat.Name == "Mana Burn Chance")
                ManaBurnChance = stat.GetValue();
            if (stat.Name == "Bleed Chance")
                BleedChance = stat.GetValue();
            if (stat.Name == "Poison Chance")
                PoisonChance = stat.GetValue();
            if (stat.Name == "Withering Chance")
                WitheringChance = stat.GetValue();
            if (stat.Name == "Ignite Chance")
                IgniteChance = stat.GetValue();
            if (stat.Name == "Max Health Damage")
                MaxHealthDamage = stat.GetValue();
            if (stat.Name == "Current Health Damage")
                CurrentHealthDamage = stat.GetValue();
            if (stat.Name == "Attack Pair Chance")
                AttackPairChance = stat.GetValue();
            if (stat.Name == "Attack Block Chance")
                AttackBlockChance = stat.GetValue();
            if (stat.Name == "Armor Penetration")
                ArmorPenetration = stat.GetValue();
            if (stat.Name == "Magic Resist Penetration")
                MagicResistPenetration = stat.GetValue();
            if (stat.Name == "Life Steal")
                LifeSteal = stat.GetValue();
            if (stat.Name == "Spell Vampirism")
                SpellVampirism = stat.GetValue();
            if (stat.Name == "Luck")
                Luck = stat.GetValue();
            if (stat.Name == "Health Regen Rate")
                HealthRegenRate = stat.GetValue();
            if (stat.Name == "Mana Regen Rate")
                ManaRegenRate = stat.GetValue();
        }
    }
    
    #endregion
}
