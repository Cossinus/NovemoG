using System;
using Novemo.Items;
using UnityEngine;

namespace Novemo.Player
{
    public class ItemPickup : Interactable
    {
        public SpriteRenderer itemSprite;
        
        public Item item;
        
        public int amount = 1;

        private Inventory.Inventory _inventory;

        private void Start()
        {
            _inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory.Inventory>();
        }

        private void LateUpdate()
        {
            itemSprite.sprite = item.itemIcon;
            itemSprite.drawMode = SpriteDrawMode.Sliced;
            var itemSpriteSize = itemSprite.size;
            itemSpriteSize.x = 0.1f;
            itemSpriteSize.y = 0.1f;
            itemSprite.size = itemSpriteSize;
        }

        public override void Interact()
        {
            base.Interact();

            PickUp();
        }

        void PickUp()
        {
            var tmpAmount = amount;
            for (var i = 0; i < tmpAmount; i++)
            {
                var wasAdded = _inventory.AddItem(item);
                amount--;
                if (wasAdded && i == tmpAmount - 1)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}