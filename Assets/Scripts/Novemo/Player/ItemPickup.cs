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
            for (var i = 0; i < amount; i++)
            {
                Inventory.Inventory.Instance.AddItem(item);

                if (i == amount - 1)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}