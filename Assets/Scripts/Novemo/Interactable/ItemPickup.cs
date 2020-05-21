using System;
using Novemo.Items;
using UnityEngine;

namespace Novemo.Interactable
{
    public class ItemPickup : Interactable
    {
        public SpriteRenderer itemSprite;
        
        public Item item;
        
        public int amount = 1;

        public DateTime dropTime;
        
        private Inventories.Inventory _inventory;

        private void Start()
        {
            _inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventories.Inventory>();
        }

        public void OnEnable()
        {
            //TODO check for amount and then pick corresponding itemSprite (e.g. 6 items of 9 stackLimit = itemIcon2, 3 items of 9 stackLimit = itemIcon1)
            //TODO merge close drops unless stacklimit is hit
            Invoke(nameof(SetValues), 0.01f);
        }

        private void SetValues()
        {
            itemSprite.sprite = item.itemIcon;
            itemSprite.drawMode = SpriteDrawMode.Sliced;
            var itemSpriteSize = itemSprite.size;

            if (item.itemSubType == ItemSubType.Sword || item.itemSubType == ItemSubType.Dagger || item.itemSubType == ItemSubType.Arrow)
            {
                itemSpriteSize.x = 0.05f;
                itemSpriteSize.y = 0.1f;
            }
            else
            {
                itemSpriteSize.x = 0.1f;
                itemSpriteSize.y = 0.1f;
            }
            
            itemSprite.size = itemSpriteSize;
        }

        public override void Interact()
        {
            if (dropTime.AddSeconds(1.5f) < DateTime.UtcNow)
                PickUp();
        }

        private void PickUp()
        {
            var tmpAmount = amount;
            for (var i = 0; i < tmpAmount; i++)
            {
                var wasAdded = _inventory.AddItem(Instantiate(item));
                if (wasAdded)
                {
                    amount--;
                    if (i == tmpAmount - 1)
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    return;
                }
            }
        }
    }
}