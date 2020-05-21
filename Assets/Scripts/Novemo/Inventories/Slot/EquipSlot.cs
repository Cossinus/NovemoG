using UnityEngine;

namespace Novemo.Inventories.Slot
{
    public class EquipSlot : Slot
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
