using System;
using System.Collections.Generic;
using System.Linq;
using Novemo.Characters.Player;
using Novemo.Crafting;
using Novemo.Items.Equipments;
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
            var stats = string.Empty;
            var levelText = string.Empty;
            var itemDescriptionText = string.Empty;
            var newLine = Environment.NewLine;
            var specialsString = newLine;
            var color = Metrics.GetStringWithRarity(itemRarity, itemName);

            if (itemDescription != string.Empty)
                itemDescriptionText = $"{newLine}{itemDescription}";

            if (_item.itemType == ItemType.Equipment)
            {
                var eq = (Equipment) _item;
                _modifiers = eq.modifiers;
            }
            
            if (_modifiers.Count > 0)
                stats += $"{newLine}Stats:";
            stats = _modifiers.Where(modifier => modifier.value > 0).Aggregate(stats, (current, modifier) => $"{current}{newLine}+{modifier.value} {modifier.name}");

            if (level > 0)
            {
                specialsString = string.Empty;
                
                if (PlayerManager.Instance.player.GetComponent<Player>().level < level)
                    levelText = $"{newLine}{newLine}<color=red><b>Level: {level}</b></color>";
                if (PlayerManager.Instance.player.GetComponent<Player>().level >= level)
                    levelText = $"{newLine}{newLine}<color=green>Level: {level}</color>";
            }
            
            if (specials != string.Empty)
                specialsString += $"{newLine}<color=#999900><size=20><i>{specials}</i></size></color>";

            //TODO display statuseffect info
            return $"{color}<size=24><color=purple><i>{itemDescriptionText}</i></color>{stats}{levelText}{specialsString}</size=24>";
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