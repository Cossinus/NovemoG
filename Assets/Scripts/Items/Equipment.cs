using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Items/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;
    
    public int armorModifier;
    public int damageModifier;

    public override void Use()
    {
        base.Use();
        EquipmentManager.Instance.Equip(this);
        RemoveFromInventory();
    }
}

// Weapon is for classes that uses only one weapon
// Shield is for Warriors and Tanks (shields are not usable, they just give stats and effects)
// Dagger1 and Dagger2 are for Assassins, Elf's, Hunters, Vikings
public enum EquipmentSlot
{
    Head, Chest, Legs, Feet, Weapon, Shield, Dagger1, Dagger2, Ring, Necklace, Rune1, Rune2, Rune3, Pet
}