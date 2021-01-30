using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Inventory inv;

    public RectTransform discardPanel = null;

    public int itemID;
    public Item item;

    [SerializeField] Image image = null;

    private Vector3 startLocation = Vector3.zero;
    private Color startColor = Color.white;

    private Color dragging = Color.red;

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;       
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startLocation = transform.position;
        image.color = dragging;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (MouseOverDiscard()) 
        {
            inv.RemoveItemFromInventory(itemID, item);
            Destroy(gameObject);
        }else 
        {
            transform.position = startLocation;
            image.color = startColor;
        }
    }

    private bool MouseOverDiscard()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        
        for (int x = 0; x < results.Count; x++)
        {
            if (results[x].gameObject.GetComponent<RectTransform>() == discardPanel) 
            {
                return true;
            }
        }

        return false;
    }
}
