using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Inventory inv = null;

    [SerializeField] Image image = null; 

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inv.itemBeingDragged) 
        {
            image.color = new Color(1f, 0f, 0f, 0.5f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (inv.itemBeingDragged)
        {
            image.color = new Color(255f, 255f, 255f, 1f);
        }
    }
}
