using UnityEngine;
using UnityEngine.EventSystems;

public class UseSlot : MonoBehaviour, IPointerClickHandler
{
    public GameObject clicked;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        var slot = gameObject.GetComponentInParent(typeof(Slot)) as Slot;

        if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover") &&
            Inventory.Instance.inventoryUI.activeInHierarchy)
        {
            if (slot.CurrentItem.type != ItemType.Material)
            {
                InventoryManager.Instance.toolTipObject.SetActive(false);
                slot.UseItemFromInventory();
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Middle && slot.IsMoreThanOneInSlot && !slot.IsEmpty &&
                 !GameObject.Find("Hover"))
        {
            Vector2 position;
            InventoryManager.Instance.clicked = clicked;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                InventoryManager.Instance.canvas.transform as RectTransform, Input.mousePosition,
                InventoryManager.Instance.canvas.worldCamera, out position);

            position.x -= InventoryManager.Instance.selectStackSize.GetComponent<RectTransform>().rect.width / 2;
            
            InventoryManager.Instance.selectStackSize.SetActive(true);
            InventoryManager.Instance.selectStackSize.transform.position =
                InventoryManager.Instance.canvas.transform.TransformPoint(position);

            InventoryManager.Instance.SetStackInfo(slot.Items.Count);
        }
    }
}