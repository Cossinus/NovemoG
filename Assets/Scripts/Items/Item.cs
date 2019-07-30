using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public new string name = "New Item";
    public Sprite icon;
    public ItemType type;
    public Rarity rarity;
    public bool isDefaultItem;
    public bool isStackable;
    public int stackLimit = 1;
    public int value;

    public virtual void Use()
    {
        Debug.Log(name + " has been used!");
        //Use item according to it's Type
    }

    public void RemoveFromInventory()
    {
        //Inventory.Instance.Remove(this); //Add Remove method in Inventory script
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