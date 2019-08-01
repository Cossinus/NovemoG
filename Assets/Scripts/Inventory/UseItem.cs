using UnityEngine;
using UnityEngine.EventSystems;

public class UseItem : MonoBehaviour, IPointerClickHandler
{
    public static bool itemButtonPressed { get; set; }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover") && Inventory.Instance.canvasGroup.alpha > 0)
        {
            itemButtonPressed = true;
        }
    }
}
