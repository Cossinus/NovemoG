using UnityEngine;

namespace Novemo.Inventories.Slots
{
    public class EquipSlot : Slots.Slot
    {
        public string useButton;

        public int equipSlotIndex;

        private void Start() => isEquipSlot = true;

        private void Update()
        {
            if (Input.GetButtonDown(useButton) && CurrentEquipment != null) CurrentEquipment.activeEffect?.Use();
        }
    }
}
