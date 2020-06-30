using System.Collections.Generic;
using JetBrains.Annotations;
using Novemo.Characters.Player;
using Novemo.StatusEffects;
using Novemo.UI;
using UnityEngine;

namespace Novemo.Items.Equipments
{
    [CreateAssetMenu(fileName = "New Equipment", menuName = "Items/Equipment/Equipment")]
    public class Equipment : Item
    {
        public EquipmentSlot equipSlot;
        
        public List<Modifier> modifiers = new List<Modifier>();
        
        [Header("Armour/Weapon/Ranged components")]
        public Item edge;
        public Item core;
        public Item guard;
        public Item grip;
        public Item limb;
        public Item @string;
        
        [CanBeNull] public ActiveEffect activeEffect;
        
        [CanBeNull] public List<StatusEffect> passiveEffects = new List<StatusEffect>();

        public float CurrentExperience { get; private set; }
        public float RequiredExperience { get; private set; }

        public override void OnEnable()
        {
            base.OnEnable();
            RequiredExperience = level * (float) itemRarity / 100; //Swap with leveling algorithm
        }

        public override bool Use()
        {
            base.Use();
            
            if (PlayerManager.Instance.player.GetComponent<Player>().level >= level)
            {
                EquipmentManager.Instance.Equip(this);
                return true;
            }
            else
            {
                EventLog.Instance.RaiseEventLog("<color=red>Your level is too low to use this item!</color>");
            }

            return false;
        }

        public void AddExperienceToEquipment(float amount)
        {
            CurrentExperience += amount;

            if (CurrentExperience >= RequiredExperience)
            {
                level += 1;
                LevelUpModifiers(1.05f);
                
                CurrentExperience -= RequiredExperience;
            }
        }

        public void LevelUpModifiers(float percentage)
        {
            for (var index = 0; index < modifiers.Count; index++)
            {
                modifiers[index] = new Modifier {name = modifiers[index].name, value = modifiers[index].value * percentage};
            }
        }

        public void BoostModifier(int index, float percentage)
        {
            modifiers[index] = new Modifier {name = modifiers[index].name, value = modifiers[index].value * percentage};
        }

        public void AddModifier(string modifierName, float modifierValue)
        {
            modifiers.Add(new Modifier{name = modifierName, value = modifierValue});
        }
        
        public void RemoveModifier(int index)
        {
            modifiers.RemoveAt(index);
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