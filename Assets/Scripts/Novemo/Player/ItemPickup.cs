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

        public void OnEnable()
        {
            Invoke(nameof(SetGFX), 0.05f);
        }

        private void SetGFX()
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
            base.Interact();

            PickUp();
        }

        void PickUp()
        {
            var tmpAmount = amount;
            for (var i = 0; i < tmpAmount; i++)
            {
                var wasAdded = _inventory.AddItem(item);
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