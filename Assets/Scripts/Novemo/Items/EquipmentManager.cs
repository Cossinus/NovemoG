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

        void Awake()
        {
            Instance = this;
        
            _inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory.Inventory>();
        
            int equipmentSlots = Enum.GetNames(typeof(EquipmentSlot)).Length;
            currentEquipment = new Equipment[equipmentSlots];
            currentEquipmentWithEffect = new Equipment[equipmentSlots];
        }
    
        #endregion

        private Inventory.Inventory _inventory;

        public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
        public OnEquipmentChanged onEquipmentChanged;
    
        [NonSerialized] public Equipment[] currentEquipment;
        [NonSerialized] public Equipment[] currentEquipmentWithEffect;

        public void Equip(Equipment newItem)
        {
            var slotIndex = (int)newItem.equipSlot;

            Equipment oldItem = null;

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
            
            if (currentEquipment[slotIndex] != null)
            {
                oldItem = currentEquipment[slotIndex];
                _inventory.AddItem(oldItem);
            }
            
            onEquipmentChanged?.Invoke(newItem, oldItem);
            
            /*if (newItem.effects.Count > 0)
            {
                currentEquipmentWithEffect[slotIndex] = newItem;
            }*/
            
            currentEquipment[slotIndex] = newItem;
            currentEquipment[slotIndex].IsEquipped = true;
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
                
                Equipment oldItem = currentEquipment[slotIndex];
                _inventory.AddItem(oldItem);
                
                //currentEquipmentWithEffect[slotIndex] = null;
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

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                UnequipAll();
            }
        }
    }
}
