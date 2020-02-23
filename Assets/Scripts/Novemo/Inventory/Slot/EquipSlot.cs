using Novemo.Items;

namespace Novemo.Inventory.Slot
{
    public class EquipSlot : Slot
    {
        public int equipSlotIndex;

        private void Start()
        {
            isEquipSlot = true;
        }
    }
}
