using System.Collections.Generic;
using Novemo.Items;
using UnityEngine;

namespace Novemo.Inventory.Slot
{
    public class EquipSlot : Slot
    {
        public int equipSlotIndex;

        void Start()
        {
            isEquipSlot = true;
        }
    }
}
