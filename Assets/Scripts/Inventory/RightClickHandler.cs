using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RightClickHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData) {
        
        if (eventData.button == PointerEventData.InputButton.Right) {
            //display functions that u can do with an item
        }
    }
    //Make ActionButton invisible
}
