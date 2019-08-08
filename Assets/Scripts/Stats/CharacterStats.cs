using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterStats : MonoBehaviour
{
    public List<Stat> stats = new List<Stat>();
    public float CurrentHealth { get; private set; }
    public float CurrentMana { get; private set; }

    public event Action<float, float> OnHealthChanged;
    
    void Awake()
    {
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
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(float damage)
    {
        float armor = stats[3].GetValue();
        damage -= armor; // Swap this with own formula (according to other effects, potions, scrolls, armor mostly in %)
        damage = Mathf.Clamp(damage, 0, float.MaxValue);
        
        CurrentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if (OnHealthChanged != null)
        {
            OnHealthChanged(stats[0].GetValue(), CurrentHealth);
        }
        
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
}
