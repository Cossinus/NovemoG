using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterStats : MonoBehaviour
{
    public List<Stat> stats = new List<Stat>();
    public float CurrentHealth { get; private set; }
    public float CurrentMana { get; private set; }
    public bool IsRegenHealth { get; set; }
    
    public float Level { get; private set; }

    private float MaxHealth;
    private float MaxMana;
    private float Armor;
    
    public event Action<float, float> OnHealthChanged;
    
    void Awake()
    {
        MaxHealth = stats[0].GetValue();
        MaxMana = stats[1].GetValue();
        Armor = stats[3].GetValue();
        
        foreach (var stat in stats)
        {
            if (stat.Name == "Health")
                CurrentHealth = stat.GetValue();
            if (stat.Name == "Mana")
                CurrentMana = stat.GetValue();
            if (stat.Name == "Movement Speed")
                GetComponent<NavMeshAgent>().speed = stat.GetValue();
        }
    }

    void Update()
    {
        if (CurrentHealth != MaxHealth && !IsRegenHealth)
            StartCoroutine(HealthRegen(stats[7].GetValue(), stats[28].GetValue()));
        
        if (Input.GetKeyDown(KeyCode.T))
            TakeDamage(20);
    }

    public void TakeDamage(float damage)
    {
        float armor = Armor;
        damage -= armor; // Swap this with own formula (according to other effects, potions, scrolls, armor mostly in %)
        damage = Mathf.Clamp(damage, 0, float.MaxValue);
        
        CurrentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

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

    IEnumerator HealthRegen(float regenValue, float healthRegenRate)
    {
        IsRegenHealth = true;
        while (CurrentHealth < MaxHealth)
        {
            CurrentHealth += regenValue;
            OnHealthChanged?.Invoke(stats[0].GetValue(), CurrentHealth);
            yield return new WaitForSeconds(healthRegenRate);
        }
        IsRegenHealth = false;
    }
}
