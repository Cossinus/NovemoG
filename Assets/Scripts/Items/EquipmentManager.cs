using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    
    public static EquipmentManager Instance;

    void Awake()
    {
        Instance = this;
        
        _inventory = Inventory.Instance;
        
        int equipmentSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        _currentEquipment = new Equipment[equipmentSlots];
    }
    
    #endregion

    private Inventory _inventory;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;
    
    private Equipment[] _currentEquipment;

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;

        Equipment oldItem = null;

        if (_currentEquipment[slotIndex] != null)
        {
            _currentEquipment[slotIndex].IsEquipped = true;
            oldItem = _currentEquipment[slotIndex];
            _inventory.AddItem(oldItem);
        }

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
        
        _currentEquipment[slotIndex] = newItem;
    }

    public void Unequip(int slotIndex)
    {
        if (_currentEquipment[slotIndex] != null && _inventory.emptySlots > 0)
        {
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
