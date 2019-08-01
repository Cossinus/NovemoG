using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
{
    public Stack<Item> Items { get; set; } = new Stack<Item>();

    public Image icon;
    public Text stackAmount;

    public bool IsAvailable => CurrentItem.stackLimit > Items.Count;

    public Item CurrentItem => Items.Peek();

    public bool IsNullOrEmpty => Items.Count == 0;

    public void AddItem(Item item)
    {
        Items.Push(item);
        if (Items.Count > 1) {
            stackAmount.text = Items.Count.ToString();
            stackAmount.gameObject.SetActive(true);
        }
        
        icon.sprite = item.icon;
        icon.gameObject.SetActive(true);
    }
    
    public void AddItems(Stack<Item> items)
    {
        Items = new Stack<Item>(items);
        
        stackAmount.text = Items.Count > 1 ? Items.Count.ToString() : string.Empty;
        stackAmount.gameObject.SetActive(true);
        
        if (Items?.Peek()) {
            icon.sprite = CurrentItem.icon;
            icon.gameObject.SetActive(true);
        } else {
            icon.sprite = null;
            icon.gameObject.SetActive(false);
        }
    }
    
    public void UseItemFromInventory()
    {
        if (!IsNullOrEmpty) {
            Items.Pop().Use();

            stackAmount.text = Items.Count > 1 ? Items.Count.ToString() : string.Empty;

            if (IsNullOrEmpty) {
                icon.sprite = null;
                icon.gameObject.SetActive(false);
                Inventory.EmptySlots++;
            }
        }
    }

    public void ClearSlot()
    {
        Items.Clear();
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        stackAmount.text = string.Empty;
    }

    public Stack<Item> RemoveItems(int amount)
    {
        Stack<Item> tmp = new Stack<Item>();
        for (int i = 0; i < amount; i++) {
            tmp.Push(Items.Pop());
        }

        Inventory.Instance.stackTxt.text = Items.Count > 1 ? Items.Count.ToString() : string.Empty;

        return tmp;
    }

    public Item RemoveItem()
    {
        var tmp = Items.Pop();
        Inventory.Instance.stackTxt.text = Items.Count > 1 ? Items.Count.ToString() : string.Empty;
        return tmp;
    }

    void Update()
    {
        if (UseSlot.ItemButtonPressed) {
            UseItemFromInventory();
        }
    }
}
