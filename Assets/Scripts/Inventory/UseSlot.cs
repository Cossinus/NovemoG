using UnityEngine;
using UnityEngine.EventSystems;

public class UseSlot : Slot, IPointerClickHandler
{
    public static bool ItemButtonPressed { get; set; }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover") && Inventory.Instance.canvasGroup.alpha > 0) {
            ItemButtonPressed = true;
        } else if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift) &&
                   Items.Count > 0 && !IsNullOrEmpty && !GameObject.Find("Hover")) {
            Vector2 position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                Inventory.Instance.canvas.transform as RectTransform, Input.mousePosition,
                Inventory.Instance.canvas.worldCamera, out position);
            
            Inventory.Instance.selectStackSize.SetActive(true);
            Inventory.Instance.selectStackSize.transform.position =
                Inventory.Instance.canvas.transform.TransformPoint(position);
            
            Inventory.Instance.SetStackInfo(Items.Count);
        }
    }
}
