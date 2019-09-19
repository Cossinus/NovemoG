using Novemo.Items;

namespace Novemo.Player
{
    public class ItemPickup : Interactable
    {
        public Item item;
        public int amount = 1;

        public override void Interact()
        {
            base.Interact();

            PickUp();
        }

        void PickUp()
        {
            var wasPickedUp = Inventory.Inventory.Instance.AddItem(item);

            if (wasPickedUp)
                Destroy(gameObject);
        }
    }
}