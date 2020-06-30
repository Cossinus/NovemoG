using System;
using Novemo.Inventories;
using Novemo.Inventories.Slots;
using UnityEngine;
using UnityEngine.UI;

namespace Novemo.Items.Equipments
{
    public class EquipmentManager : MonoBehaviour
    {
        #region Singleton
    
        public static EquipmentManager Instance;

        private void Awake()
        {
            Instance = this;
        
            _inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventories.Inventory>();
        
            var equipmentSlots = Enum.GetNames(typeof(EquipmentSlot)).Length;
            currentEquipment = new Equipment[equipmentSlots];
        }
    
        #endregion

        private Inventory _inventory;

        public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
        public OnEquipmentChanged onEquipmentChanged;
    
        [SerializeField] [HideInInspector] public Equipment[] currentEquipment;

        [NonSerialized] public Equipment tmpExchangeEquipment;
        
        public void Equip(Equipment newItem)
        {
            tmpExchangeEquipment = null;
            
            var slotIndex = (int)newItem.equipSlot;

            if (currentEquipment[slotIndex] != null)
            {
                if (_inventory.EmptySlots == 0)
                {
                    tmpExchangeEquipment = currentEquipment[slotIndex];
                }
                Unequip(slotIndex);
            }
            
            foreach (var slot in EquipmentPanel.Instance.allEquipSlots)
            {
                if ((int) newItem.equipSlot == slot.equipSlotIndex)
                {
                    var equipSlot = slot.GetComponent<EquipSlot>();

                    if (currentEquipment[slotIndex] != null) equipSlot.ClearSlot();

                    equipSlot.icon.GetComponent<Image>().sprite = newItem.itemIcon;
                    equipSlot.icon.gameObject.SetActive(true);
                    equipSlot.AddItemToSlot(newItem);
                }
            }

            currentEquipment[slotIndex] = newItem;

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
                        equipSlot.RemoveSlotItem();
                    }
                }
                
                var oldItem = currentEquipment[slotIndex];
                _inventory.AddItem(oldItem);

                currentEquipment[slotIndex] = null;

                onEquipmentChanged?.Invoke(null, oldItem);
            }
        }

        public void UnequipAll()
        {
            for (var i = 0; i < currentEquipment.Length; i++)
                if (_inventory.EmptySlots > 0) Unequip(i);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U)) UnequipAll();
        }
    }
}
