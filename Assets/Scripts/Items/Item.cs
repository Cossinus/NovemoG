using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public string itemName = "New Item";
    public string description = "Default Description";
    public Sprite icon;
    public ItemType type;
    public Rarity rarity;
    public bool isDefaultItem;
    public bool isStackable;
    public int stackLimit = 1;
    public int value;

    [NonSerialized] protected int Armor;
    [NonSerialized] protected int Damage;

    public virtual void Use()
    {
        Debug.Log(itemName + " has been used!");
        //Use item according to it's Type
    }

    public void RemoveFromInventory()
    {
        //Inventory.Instance.Remove(this); //Add Remove method in Inventory script
    }
    
    public string GetTooltip()
    {
        string stats = string.Empty;
        string color = string.Empty;
        string newLine = string.Empty;

        if (description != string.Empty)
        {
            newLine = "\n";
        }

        switch (rarity)
        {
            case Rarity.Common:
                color = "gray";
                break;
            case Rarity.Normal:
                color = "yellow";
                break;
            case Rarity.Uncommon:
                color = "lime";
                break;
            case Rarity.Rare:
                color = "brick";
                break;
            case Rarity.VeryRare:
                color = "navy";
                break;
            case Rarity.Epic:
                color = "orange";
                break;
            case Rarity.Legendary:
                color = "magenta";
                break;
            case Rarity.Mystical:
                color = "red";
                break;
            case Rarity.Artifact:
                color = "white";
                break;
        }

        if (Armor > 0)
        {
            stats += "\n+" + Armor.ToString() + " Strength";
        }
        if (Damage > 0)
        {
            stats += "\n+" + Damage.ToString() + " Damage";
        } //TODO change that

        return string.Format(
            "<color=" + color + "><size=16>{0}</size></color><size=14><i><color=purple>" + newLine +
            "{1}</color></i>{2}</size>", itemName, description, stats);
    }
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
    
    //etc...
    //Only types of things that are used to craft something or use elsewhere (e.g. quest items)
}