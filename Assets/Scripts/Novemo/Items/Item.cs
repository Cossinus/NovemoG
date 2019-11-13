using System;
using System.Collections.Generic;
using Novemo.Crafting;
using UnityEngine;

namespace Novemo.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
    public class Item : ScriptableObject
    {
        public string itemName = "New Item";
        [TextArea(2, 5)]
        public string itemDescription = "Item Description";
        public string specials = string.Empty;
        public string craftName = string.Empty;

        public int stackLimit = 1;
        public int value;

        public bool isDiscovered;

        public Recipe recipe;

		public Sprite itemIcon;
        
        public ItemType itemType;
        public Rarity itemRarity;

        public int level;
        public float CurrentExperience { get; set; }
        public float RequiredExperience { get; set; }
        
        #region GetModifiers
        private Item _item;
        private List<Modifier> _modifiers = new List<Modifier>();
        #endregion

        private void OnEnable()
        {
            _item = this; 
            RequiredExperience = level * (float) itemRarity / 100;
        }
        
        public virtual void Use()
        {
            Debug.Log($"{itemName} has been used!");
        }

        public string GetTooltip()
        {
            var stats = string.Empty;
            var color = string.Empty;
            var newLine = string.Empty;
            var itemLevel = level.ToString();

            if (itemDescription != string.Empty)
                newLine = "\n";

            if (itemLevel == "1" || itemLevel == "0")
                itemLevel = string.Empty;
            else
                itemLevel = $"Level: {level}";

            if (_item.itemType == ItemType.Equipment)
            {
                var eq = (Equipment) _item;
                _modifiers = eq.modifiers;
            }
            
            foreach (var modifier in _modifiers)
            {
                if (modifier.Value > 0)
                    stats += $"{Environment.NewLine}+{modifier.Value} {modifier.Name}";
            }
            
            switch (itemRarity)
            {
                case Rarity.Common:
                    color = $"#696969><size=40>{itemName}</size>";
                    break;
                case Rarity.Normal:
                    color = $"yellow><size=40>{itemName}</size>";
                    break;
                case Rarity.Uncommon:
                    color = $"#bfff00><size=40>{itemName}</size>";
                    break;
                case Rarity.Rare:
                    color = $"#bc3c21><size=40>{itemName}</size>";
                    break;
                case Rarity.VeryRare:
                    color = $"#00CED1><size=40>{itemName}</size>";
                    break;
                case Rarity.Epic:
                    color = $"orange><size=40><b>{itemName}</b></size>";
                    break;
                case Rarity.Legendary:
                    color = $"#ff00ff><size=40><b>{itemName}</b></size>";
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
                    "<color={4}</color><size=24><i><color=purple>{5}{0}</color></i>\nStats:{1}\n" +
                    "\n<color=#999900><size=20><i>{2}</i></size></color>" /* +
                effect.EffectText()*/, itemDescription, stats, specials, itemLevel, color, newLine);
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
    }

    [Serializable]
    public struct Modifier
    {
        public string Name;
        public float Value;
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
    
        // etc...
        // Only types of things that are used to craft something or use elsewhere (e.g. quest items)
    }
}