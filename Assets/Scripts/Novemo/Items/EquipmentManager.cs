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
        
            _inventory = Novemo.Inventory.Inventory.Instance;
        
            int equipmentSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
            _currentEquipment = new Equipment[equipmentSlots];
        }
    
        #endregion

        private Novemo.Inventory.Inventory _inventory;

        private EquipmentPanel _equipmentPanel;

        public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
        public OnEquipmentChanged onEquipmentChanged;
    
        private Equipment[] _currentEquipment;

        public void Equip(Equipment newItem)
        {
            int slotIndex = (int)newItem.equipSlot;

            Equipment oldItem = null;

            foreach (var slot in EquipmentPanel.Instance.allEquipSlots)
            {
                if ((int) newItem.equipSlot == slot.equipSlotIndex)
                {
                    var equipSlot = slot.GetComponent<EquipSlot>();
                
                    equipSlot.icon.GetComponent<Image>().sprite = newItem.icon;
                    equipSlot.icon.gameObject.SetActive(true);
                    equipSlot.AddItem(newItem);

                    if (_currentEquipment[slotIndex] != null)
                        equipSlot.RemoveItem();
                }
            }
        
            if (_currentEquipment[slotIndex] != null)
            {
                _currentEquipment[slotIndex].IsEquipped = true;
                oldItem = _currentEquipment[slotIndex];
                _inventory.AddItem(oldItem);
            }

            onEquipmentChanged?.Invoke(newItem, oldItem);

            _currentEquipment[slotIndex] = newItem;
        }

        public void Unequip(int slotIndex)
        {
            if (_currentEquipment[slotIndex] != null && _inventory.emptySlots > 0)
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
            
                _currentEquipment[slotIndex].IsEquipped = false;
                Equipment oldItem = _currentEquipment[slotIndex];
                _inventory.AddItem(oldItem);

                _currentEquipment[slotIndex] = null;

                onEquipmentChanged?.Invoke(null, oldItem);
            }
        }

        public void UnequipAll()
        {
            for (int i = 0; i < _currentEquipment.Length; i++)
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
