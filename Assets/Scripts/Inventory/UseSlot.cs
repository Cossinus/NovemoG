using UnityEngine;
using UnityEngine.EventSystems;

public class UseSlot : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Slot slot = gameObject.GetComponentInParent(typeof(Slot)) as Slot;
        
        if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover") && Inventory.Instance.inventoryUI.activeInHierarchy) {
            slot.UseItemFromInventory();
        } else if (eventData.button == PointerEventData.InputButton.Middle && slot.IsMoreThanOneInSlot && !slot.IsEmpty && !GameObject.Find("Hover")) {
            Vector2 position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                Inventory.Instance.canvas.transform as RectTransform, Input.mousePosition,
                Inventory.Instance.canvas.worldCamera, out position);
            
            Inventory.Instance.selectStackSize.SetActive(true);
            Inventory.Instance.selectStackSize.transform.position =
                Inventory.Instance.canvas.transform.TransformPoint(position);
            
            Inventory.Instance.SetStackInfo(slot.Items.Count);
        }
    }
}
