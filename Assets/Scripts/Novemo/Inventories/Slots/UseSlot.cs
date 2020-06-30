using Novemo.Items;
using Novemo.Items.Equipments;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Novemo.Inventories.Slots
{
    public class UseSlot : MonoBehaviour, IPointerClickHandler
    {
        public GameObject clicked;
        
        private Inventory _inventory;
        private InventoryManager _inventoryManager;

        private void Start()
        {
            _inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
            _inventoryManager = InventoryManager.Instance;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var slot = (Slot) gameObject.GetComponentInParent(typeof(Slot));

            if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover") && 
                !clicked.GetComponent<Slot>().IsEmpty && _inventory.canvasGroup.alpha > 0)
            {
                if (slot.CurrentItem.itemType != ItemType.Material && slot.isEquipSlot == false)
                {
                    _inventoryManager.toolTipObject.SetActive(false);
                    slot.UseItemFromSlot();
                    if (EquipmentManager.Instance.tmpExchangeEquipment != null)
                    {
                        _inventory.AddItem(EquipmentManager.Instance.tmpExchangeEquipment);
                        return;
                    }
                    if (slot.Items.Count > 0 && slot.CurrentItem.itemType == ItemType.Equipment)
                    {
                        _inventory.EmptySlots++;
                    }
                }

                if (clicked.CompareTag("EquipSlot"))
                {
                    var equipSlotIndex = clicked.GetComponent<EquipSlot>().equipSlotIndex;
                    EquipmentManager.Instance.Unequip(equipSlotIndex);
                    _inventory.HideToolTip();
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Middle && slot.IsMoreThanOneInSlot && !slot.IsEmpty &&
                     !GameObject.Find("Hover") && _inventory.canvasGroup.alpha > 0)
            {
                _inventoryManager.Clicked = clicked;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _inventoryManager.canvas.transform as RectTransform, Input.mousePosition,
                    _inventoryManager.canvas.worldCamera, out var position);

                position.x -= _inventoryManager.selectStackSize.GetComponent<RectTransform>().rect.width / 2;
            
                _inventoryManager.selectStackSize.SetActive(true);
                _inventoryManager.selectStackSize.transform.position =
                    _inventoryManager.canvas.transform.TransformPoint(position);

                _inventoryManager.SetStackInfo(slot.Items.Count);
            }
        }
    }
}