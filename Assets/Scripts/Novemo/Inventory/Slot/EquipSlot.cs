using Novemo.Items;

namespace Novemo.Inventory.Slot
{
    public class EquipSlot : Slot
    {
        public int equipSlotIndex;

        /*public Equipment CurrentEquipment
        {
            get
            {
                var allEquipment = EquipmentManager.Instance.currentEquipment;
                foreach (var equipped in allEquipment)
                {
                    if (equipped != null)
                    {
                        return equipped;
                    }
                }
                
                return null;
            }
        }*/
        
        void Start()
        {
            isEquipSlot = true;
        }
    }
}
