using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : ClassManager
{
    public string passiveName;
    public string passiveDescription;
    
    void Awake()
    {
        className = "Warrior";
        classDescription = "";
        passiveName = "Thick Skin";
        passiveDescription = "Gives you (<color=#ff3232><b>+3%</b></color> max HP) " +
                             "and from the start player has (<color=#ff3232><b>25 HP</b></color>) more.";
        abilities = gameObject.AddComponent<WarriorAbilities>();
    }

    void Start()
    {
        defaultWeapons = Resources.Load<Equipment>("Items/Weapons/WarriorSword");
        EquipmentManager.Instance.Equip(defaultWeapons);
        Instance.myStats.CurrentHealth += 25;
    }

    public override void Passive()
    {
        Instance.myStats.stats[0].BaseValue += 25;
        float maxHealth = Instance.myStats.stats[0].GetValue();
        maxHealth *= .03f;
        Instance.myStats.stats[0].AddModifier(maxHealth);
    }
}
