using Novemo.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Novemo.Inventory.Slot
{
    public class UseSlot : MonoBehaviour, IPointerClickHandler
    {
        public GameObject clicked;
        
        private Inventory _inventory;

        private void Start()
        {
            _inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var slot = (Slot) gameObject.GetComponentInParent(typeof(Slot));

            if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover") && 
                !clicked.GetComponent<Slot>().IsEmpty && _inventory.canvasGroup.alpha > 0)
            {
                if (slot.CurrentItem.itemType != ItemType.Material && slot.isEquipSlot == false)
                {
                    InventoryManager.Instance.toolTipObject.SetActive(false);
                    slot.UseItemFromInventory();
                }

                if (clicked.CompareTag("EquipSlot"))
                {
                    var equipSlotIndex = clicked.GetComponent<EquipSlot>().equipSlotIndex;
                    EquipmentManager.Instance.Unequip(equipSlotIndex);
                    Inventory.Instance.HideToolTip();
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Middle && slot.IsMoreThanOneInSlot && !slot.IsEmpty &&
                     !GameObject.Find("Hover") && _inventory.canvasGroup.alpha > 0)
            {
                InventoryManager.Instance.clicked = clicked;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    InventoryManager.Instance.canvas.transform as RectTransform, Input.mousePosition,
                    InventoryManager.Instance.canvas.worldCamera, out var position);

                position.x -= InventoryManager.Instance.selectStackSize.GetComponent<RectTransform>().rect.width / 2;
            
                InventoryManager.Instance.selectStackSize.SetActive(true);
                InventoryManager.Instance.selectStackSize.transform.position =
                    InventoryManager.Instance.canvas.transform.TransformPoint(position);

                InventoryManager.Instance.SetStackInfo(slot.Items.Count);
            }
        }
    }
}