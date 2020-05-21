using System;
using System.Collections.Generic;
using Novemo.Crafting;
using UnityEngine;

namespace Novemo.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
    public class Item : ScriptableObject, IEquatable<Item>
    {
        public string itemName = "New Item";
        [TextArea(2, 5)]
        public string itemDescription = "Item Description";
        public string specials = string.Empty;

        public int stackLimit = 1;
        public int level = 1;
        public int value;

        public bool isDiscovered;

        public Recipe recipe;

        public Sprite itemIcon;
        
        public ItemType itemType;
        public ItemSubType itemSubType;
        public Rarity itemRarity;

        #region GetModifiers
        protected Item _item;
        private List<Modifier> _modifiers = new List<Modifier>();
        #endregion

        public virtual void OnEnable()
        {
            _item = this;
            SetDescription();
        }

        public virtual void SetDescription() { }

        public virtual bool Use()
        {
            Debug.Log($"{itemName} has been used!");
            return true;
        }

        public string GetTooltip()
        {
            //Use StringBuilder();
            var stats = string.Empty;
            var color = string.Empty;
            var newLine = string.Empty;

            if (itemDescription != string.Empty)
                newLine = "\n";

            if (_item.itemType == ItemType.Equipment)
            {
                var eq = (Equipment) _item;
                _modifiers = eq.modifiers;
            }
            
            foreach (var modifier in _modifiers)
            {
                if (modifier.value > 0)
                    stats = $"{stats}{Environment.NewLine}+{modifier.value} {modifier.name}";
            }
            
            switch (itemRarity)
            {
                case Rarity.Common:
                    color = $"#696969><size=40>{itemName}</size>"; //dark gray
                    break;
                case Rarity.Normal:
                    color = $"yellow><size=40>{itemName}</size>";
                    break;
                case Rarity.Uncommon:
                    color = $"#bfff00><size=40>{itemName}</size>"; //lime-ish
                    break;
                case Rarity.Rare:
                    color = $"#bc3c21><size=40>{itemName}</size>"; //brick-ish
                    break;
                case Rarity.VeryRare:
                    color = $"#00CED1><size=40>{itemName}</size>"; //cyan-ish
                    break;
                case Rarity.Epic:
                    color = $"orange><size=40><b>{itemName}</b></size>"; 
                    break;
                case Rarity.Legendary:
                    color = $"#ff00ff><size=40><b>{itemName}</b></size>"; //purple-ish
                    break;
                case Rarity.Mystical:
                    color = $"red><size=40><b>{itemName}</b></size>";
                    break;
                case Rarity.Artifact:
                    color = $"white><size=40><b>{itemName}</b></size>";
                    break;
            }

            if (stats != string.Empty && specials != string.Empty)
            {
                return string.Format(
                    "<color={3}</color><size=24><i><color=purple>{4}{0}</color></i>\nStats:{1}\n" +
                    "\n<color=#999900><size=20><i>{2}</i></size></color>" /* +
                effect.EffectText()*/, itemDescription, stats, specials, color, newLine);
            }
            else if (stats != string.Empty && specials == string.Empty)
            {
                return string.Format(
                    "<color={2}</color><size=24><i><color=purple>{3}{0}</color></i>\nStats:{1}" /* + effect.EffectText()*/,
                    itemDescription, stats, color, newLine);
            }
            else if (stats == string.Empty && specials != string.Empty)
            {
                return string.Format(
                    "<color={2}</color><size=24><i><color=purple>{3}{0}</color></i>" +
                    "\n<color=#999900><size=20><i>{1}</i></size></color>" /* +
                effect.EffectText()*/, itemDescription, specials, color, newLine);
            }
            else
            {
                return string.Format(
                    "<color={1}</color><size=24><i><color=purple>{2}{0}</color></i></size>", itemDescription, color,
                    newLine);
            }
        }

        #region Override Equals

        public override bool Equals(object obj) => Equals(obj as Item);

        public virtual bool Equals(Item other)
        {
            if (ReferenceEquals(other, null)) {
                return false;
            }
			
            if (ReferenceEquals(this, other)) {
                return true;
            }
			
            if (GetType() != other.GetType()) {
                return false;
            }

            return itemName == other.itemName && 
                   specials == other.specials && 
                   stackLimit == other.stackLimit && 
                   level == other.level && 
                   value == other.value && 
                   isDiscovered == other.isDiscovered && 
                   recipe == other.recipe && 
                   itemIcon == other.itemIcon && 
                   itemType == other.itemType && 
                   itemSubType == other.itemSubType && 
                   itemRarity == other.itemRarity;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (itemName != null ? itemName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (specials != null ? specials.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ stackLimit;
                hashCode = (hashCode * 397) ^ level;
                hashCode = (hashCode * 397) ^ value;
                hashCode = (hashCode * 397) ^ isDiscovered.GetHashCode();
                hashCode = (hashCode * 397) ^ (recipe != null ? recipe.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (itemIcon != null ? itemIcon.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) itemType;
                hashCode = (hashCode * 397) ^ (int) itemSubType;
                hashCode = (hashCode * 397) ^ (int) itemRarity;
                return hashCode;
            }
        }

        #endregion
    }

    [Serializable]
    public struct Modifier
    {
        public string name;
        public float value;
    }

    public enum Rarity {
        Common,
        Normal,
        Uncommon,
        Rare,
        VeryRare,
        Epic,
        Legendary,
        Mystical,
        Artifact
    }

    public enum ItemType {
        Equipment,
        QuestEquipment,
        Material,
        QuestMaterial,
        Potion,
        QuestPotion,
        Scroll,
        QuestScroll,
        LootChest,
        // etc...
        // Only types of things that are used to craft something or use elsewhere (e.g. quest items)
    }

    public enum ItemSubType {
        Shield,
        Sword,
        Bow, //or crossbow
        Arrow,
        Chestplate,
        Leggings,
        Boots,
        Helmet,
        Ring,
        Pet,
        Scroll,
        Potion,
        Alloy,
        Necklace,
        Rune,
        Dagger,
        CraftingMaterial,
        Bar,
        LootChest,
        Gem,
        Pickaxe,
    }
}