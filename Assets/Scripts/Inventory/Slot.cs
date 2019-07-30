﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    private Stack<Item> _items = new Stack<Item>();
    public Stack<Item> Items {
        get => _items;
        set => _items = value;
    }

    public Image icon;
    public Text stackAmount;

    public bool IsAvailable => CurrentItem.stackLimit > _items.Count;

    public Item CurrentItem => _items.Peek();

    public bool IsNullOrEmpty => _items.Count == 0;

    public void AddItem(Item item)
    {
        _items.Push(item);
        if (_items.Count > 1) {
            stackAmount.text = _items.Count.ToString();
            stackAmount.gameObject.SetActive(true);
        }
        
        icon.sprite = item.icon;
        icon.gameObject.SetActive(true);
    }
    
    public void AddItems(Stack<Item> items)
    {
        _items = new Stack<Item>(items);
        
        stackAmount.text = _items.Count > 1 ? _items.Count.ToString() : string.Empty;
        stackAmount.gameObject.SetActive(true);
        
        if (_items?.Peek()) {
            icon.sprite = CurrentItem.icon;
            icon.gameObject.SetActive(true);
        } else {
            icon.sprite = null;
            icon.gameObject.SetActive(false);
        }
    }
    
    //If I wanted to use an item by clicking on a slot,
    //I have to make OnClick event in the Inspector
    //And attach Slot + this function
    public void UseItem()
    {
        if (!IsNullOrEmpty) {
            _items.Pop().Use();

            stackAmount.text = _items.Count > 1 ? _items.Count.ToString() : string.Empty;

            if (IsNullOrEmpty) {
                icon.sprite = null;
                icon.gameObject.SetActive(false);
                Inventory.EmptySlots++;
            }
        }
    }

    public void ClearSlot()
    {
        _items.Clear();
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        stackAmount.text = string.Empty;
    }

    //Use second prefab instead
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover") && Inventory.Instance.canvasGroup.alpha > 0)
        {
            UseItem();
        }
    }
}
