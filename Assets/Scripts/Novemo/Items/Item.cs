﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Novemo.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
    public class Item : ScriptableObject
    {
        public string itemName = "New Item";
        [TextArea(2, 5)]
        public string description = "Item Description";
        public string specials = string.Empty;
        public Sprite icon;
        public ItemType type;
        public Rarity rarity;
        public bool isDefaultItem;
        public int stackLimit = 1;
        public int value;
        public List<Modifier> Modifiers = new List<Modifier>();
    
        public int level;
        public float CurrentExperience { get; set; }
        public float RequiredExperience { get; set; }

        public virtual void Use()
        {
            Debug.Log(itemName + " has been used!");
        }

        public string GetTooltip()
        {
            string stats = string.Empty;
            string color = string.Empty;
            string newLine = string.Empty;
            string itemLevel = level.ToString();

            if (description != string.Empty)
                newLine = "\n";

            if (itemLevel == "1")
                itemLevel = string.Empty;
            else
                itemLevel = $"{level} :Level";

            switch (rarity)
            {
                case Rarity.Common:
                    color = "#696969><size=40>" + itemName + "</size>";
                    break;
                case Rarity.Normal:
                    color = "yellow><size=40>" + itemName + "</size>";
                    break;
                case Rarity.Uncommon:
                    color = "#bfff00><size=40>" + itemName + "</size>";
                    break;
                case Rarity.Rare:
                    color = "#bc3c21><size=40>" + itemName + "</size>";
                    break;
                case Rarity.VeryRare:
                    color = "#00CED1><size=40>" + itemName + "</size>";
                    break;
                case Rarity.Epic:
                    color = "orange><size=40><b>" + itemName + "</b></size>";
                    break;
                case Rarity.Legendary:
                    color = "#ff00ff><size=40><b>" + itemName + "</b></size>";
                    break;
                case Rarity.Mystical:
                    color = "red><size=40><b>" + itemName + "</b></size>";
                    break;
                case Rarity.Artifact:
                    color = "white><size=40><b>" + itemName + "</b></size>";
                    break;
            }

            foreach (var modifier in Modifiers)
            {
                if (modifier.Value > 0)
                    stats += $"{Environment.NewLine}+{modifier.Value} {modifier.Name}";
            }

            if (stats != string.Empty && specials != string.Empty)
            {
                return string.Format(
                    "<color=" + color + "</color><size=24><i><color=purple>" + newLine +
                    "{0}</color></i>\nStats:{1}\n" + "\n<color=#999900><size=20><i>{2}</i></size></color>"/* +
                effect.EffectText()*/, description, stats, specials, itemLevel);
            }
            else if (stats != string.Empty && specials == string.Empty)
            {
                return string.Format(
                    "<color=" + color + "</color><size=24><i><color=purple>" + newLine +
                    "{0}</color></i>\nStats:{1}"/* + effect.EffectText()*/, description, stats);
            }
            else if (stats == string.Empty && specials != string.Empty)
            {
                return string.Format(
                    "<color=" + color + "</color><size=24><i><color=purple>" + newLine +
                    "{0}</color></i>" + "\n<color=#999900><size=20><i>{1}</i></size></color>"/* +
                effect.EffectText()*/, description, specials);
            }
            else
            {
                return string.Format(
                    "<color=" + color + "</color><size=24><i><color=purple>" + newLine +
                    "{0}</color></i></size>", description);
            }
        }

        public void SetStats(Item item)
        {
            this.itemName = item.itemName;
            this.description = item.description;
            this.specials = item.specials;
            this.icon = item.icon;
            this.type = item.type;
            this.rarity = item.rarity;
            this.stackLimit = item.stackLimit;
            this.isDefaultItem = item.isDefaultItem;
            this.value = item.value;
        
            this.Modifiers = item.Modifiers;
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