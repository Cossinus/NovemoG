using System.Collections.Generic;
using Novemo.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Novemo.Inventory.Slot
{
    public class Slot : MonoBehaviour
    {
        public Stack<Item> Items { get; set; }

        public Image icon;
        public Text stackAmount;

        public bool isEquipSlot;

        public bool IsAvailable => CurrentItem.stackLimit > Items.Count;

        public bool IsMoreThanOneInSlot => Items.Count > 1;

        public Item CurrentItem => Items?.Peek();

        public bool IsEmpty => Items.Count == 0;

        private void Awake()
        {
            Items = new Stack<Item>();
        }

        public void AddItem(Item item)
        {
            Items.Push(item);
            if (Items.Count > 1)
            {
                stackAmount.text = Items.Count.ToString();
                stackAmount.gameObject.SetActive(true);
            }

            icon.gameObject.SetActive(true);
            icon.sprite = item.icon;
        }

        public void AddItems(Stack<Item> items)
        {
            Items = new Stack<Item>(items);

            stackAmount.text = IsMoreThanOneInSlot ? Items.Count.ToString() : string.Empty;
            stackAmount.gameObject.SetActive(true);

            if (Items?.Peek())
            {
                icon.sprite = CurrentItem.icon;
                icon.gameObject.SetActive(true);
            }
            else
            {
                icon.sprite = null;
                icon.gameObject.SetActive(false);
            }
        }

        public void UseItemFromInventory()
        {
            if (!IsEmpty)
            {
                Items.Pop().Use();

                stackAmount.text = IsMoreThanOneInSlot ? Items.Count.ToString() : string.Empty;

                if (IsEmpty)
                {
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
            var tmp = new Stack<Item>();
            for (var i = 0; i < amount; i++) tmp.Push(Items.Pop());

            stackAmount.text = IsMoreThanOneInSlot ? Items.Count.ToString() : string.Empty;

            return tmp;
        }

        public Item RemoveItem()
        {
            var tmp = Items.Pop();
            stackAmount.text = IsMoreThanOneInSlot ? Items.Count.ToString() : string.Empty;
            return tmp;
        }
    }
}