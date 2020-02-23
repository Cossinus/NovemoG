using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Novemo.Items
{
    [CreateAssetMenu(fileName = "New Equipment", menuName = "Items/Equipment")]
    public class Equipment : Item
    {
        public EquipmentSlot equipSlot;
        
        public List<Modifier> modifiers = new List<Modifier>();
        
        public List<UniqueEffect> effects = new List<UniqueEffect>();
        
        public bool IsEquipped { get; set; }
        
        public float CurrentExperience { get; set; }
        public float RequiredExperience { get; set; }

        public override void OnEnable()
        {
            base.OnEnable();
            RequiredExperience = level * (float) itemRarity / 100; //Swap with leveling algorithm
        }

        public override bool Use()
        {
            base.Use();
            EquipmentManager.Instance.Equip(this);
            return true;
        }
    }

    // Weapon is for classes that uses only one weapon
    // Shield is for Warriors and Tanks (shields are not usable, they just give stats and effects)
    // Weapon + Dagger are for:
    // Assassins (2x Dirk/Dagger or Dirk + Dagger),
    // Elf's (Bow + Dagger), Hunters (Bow + Dirk),
    // Vikings (2x axes),
    // Mages (Weapon + Spellbook)
    public enum EquipmentSlot
    {
        Head, Chest, Legs, Feet, Weapon, Shield, Dagger, Ring, Necklace, Rune1, Rune2, Rune3, Pet
    }
}