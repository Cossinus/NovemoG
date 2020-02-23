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
            icon.sprite = item.itemIcon;
            SetGraphics();
        }

        public void AddItems(Stack<Item> items)
        {
            Items = new Stack<Item>(items);

            stackAmount.text = IsMoreThanOneInSlot ? Items.Count.ToString() : string.Empty;
            stackAmount.gameObject.SetActive(true);

            if (Items?.Peek())
            {
                icon.sprite = CurrentItem.itemIcon;
                icon.gameObject.SetActive(true);
            }
            else
            {
                icon.sprite = null;
                icon.gameObject.SetActive(false);
            }
            SetGraphics();
        }

        private void SetGraphics()
        {
            var rect = icon.GetComponent<RectTransform>();
            
            if (CurrentItem.itemSubType == ItemSubType.Sword || CurrentItem.itemSubType == ItemSubType.Dagger || CurrentItem.itemSubType == ItemSubType.Arrow)
            {
                rect.sizeDelta = new Vector2(60, 120);
                rect.eulerAngles = new Vector3(0, 0, -45);
            }
            else
            {
                rect.sizeDelta = new Vector2(90, 90);
                rect.eulerAngles = new Vector3(0, 0, 0);
            }
        }

        public void UseItemFromInventory()
        {
            if (IsEmpty) return;
            if (!Items.Peek().Use()) return;

            Items.Pop();

            stackAmount.text = IsMoreThanOneInSlot ? Items.Count.ToString() : string.Empty;

            if (!IsEmpty) return;
            icon.sprite = null;
            icon.gameObject.SetActive(false);
            transform.parent.parent.GetComponent<Inventory>().EmptySlots++;
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
            if (!IsEmpty)
            {
                var tmp = Items.Pop();
                stackAmount.text = IsMoreThanOneInSlot ? Items.Count.ToString() : string.Empty;

                if (IsEmpty)
                {
                    ClearSlot();
                }
                
                return tmp;
            }

            return null;
        }
    }
}