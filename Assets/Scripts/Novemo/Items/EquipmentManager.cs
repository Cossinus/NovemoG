using System;
using Novemo.Inventory;
using Novemo.Inventory.Slot;
using UnityEngine;
using UnityEngine.UI;

namespace Novemo.Items
{
    public class EquipmentManager : MonoBehaviour
    {
        #region Singleton
    
        public static EquipmentManager Instance;

        private void Awake()
        {
            Instance = this;
        
            _inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory.Inventory>();
        
            var equipmentSlots = Enum.GetNames(typeof(EquipmentSlot)).Length;
            currentEquipment = new Equipment[equipmentSlots];
        }
    
        #endregion

        private Inventory.Inventory _inventory;

        public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
        public OnEquipmentChanged onEquipmentChanged;
    
        [NonSerialized] public Equipment[] currentEquipment;

        public void Equip(Equipment newItem)
        {
            var slotIndex = (int)newItem.equipSlot;

            if (currentEquipment[slotIndex] != null)
            {
                Unequip(slotIndex);
            }
            
            foreach (var slot in EquipmentPanel.Instance.allEquipSlots)
            {
                if ((int) newItem.equipSlot == slot.equipSlotIndex)
                {
                    var equipSlot = slot.GetComponent<EquipSlot>();

                    if (currentEquipment[slotIndex] != null)
                    {
                        equipSlot.ClearSlot();
                    }

                    equipSlot.icon.GetComponent<Image>().sprite = newItem.itemIcon;
                    equipSlot.icon.gameObject.SetActive(true);
                    equipSlot.AddItem(newItem);
                }
            }

            currentEquipment[slotIndex] = newItem;
            currentEquipment[slotIndex].IsEquipped = true;
            
            onEquipmentChanged?.Invoke(newItem, null);
        }

        public void Unequip(int slotIndex)
        {
            if (currentEquipment[slotIndex] != null && _inventory.EmptySlots > 0)
            {
                foreach (var slot in EquipmentPanel.Instance.allEquipSlots)
                {
                    if (slotIndex == slot.equipSlotIndex)
                    {
                        var equipSlot = slot.GetComponent<EquipSlot>();
                    
                        equipSlot.icon.GetComponent<Image>().sprite = null;
                        equipSlot.icon.gameObject.SetActive(false);
                        equipSlot.RemoveItem();
                    }
                }
                
                var oldItem = currentEquipment[slotIndex];
                _inventory.AddItem(oldItem);
                
                currentEquipment[slotIndex].IsEquipped = false;
                currentEquipment[slotIndex] = null;

                onEquipmentChanged?.Invoke(null, oldItem);
            }
        }

        public void UnequipAll()
        {
            for (int i = 0; i < currentEquipment.Length; i++)
            {
                Unequip(i);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                UnequipAll();
            }
        }
    }
}
