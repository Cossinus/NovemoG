using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public string abilityName = "Ability Name";
    [TextArea(2, 5)]
    public string abilityDescription = "Ability Description";
    public float abilityDamage;

    public PlayerManager playerManager;
    [NonSerialized]
    public CharacterStats myStats;
    
    void Start()
    {
        playerManager = PlayerManager.Instance;
        myStats = playerManager.player.GetComponent<CharacterStats>();
    }
    
    public virtual void Use()
    {
        
    }
}
