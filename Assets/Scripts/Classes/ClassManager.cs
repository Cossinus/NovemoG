using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassManager : MonoBehaviour
{
    public static ClassManager Instance;

    void Awake()
    {
        Instance = this;
    }
    
    public string className = "New Class";
    public string classDescription = "Class Description";

    public Equipment defaultWeapons;

    public AbilityManager abilities;

    public PlayerManager playerManager;
    public CharacterStats myStats;
    
    void Start()
    {
        playerManager = PlayerManager.Instance;
        myStats = playerManager.player.GetComponent<CharacterStats>();
    }

    public virtual void Passive() { }
}
