using System.Collections;
using System.Collections.Generic;
using Novemo;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public override void Die()
    {
        base.Die();
        
        // death animation
        // drop loot
        
        
        Destroy(gameObject);
    }
}
