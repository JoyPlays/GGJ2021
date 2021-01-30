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

    private bool thisItemBeingDragged = false;

    private int startItemX = 0, startItemY = 0;
    private Vector3 startLocation = Vector3.zero;
    private Color startColor;

    private Color dragging = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    private void Start () 
    {
        startItemX = item.sizeX;
        startItemY = item.sizeY;
        startColor = image.color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && thisItemBeingDragged)
        {
            RotateItem();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;

        inv.SelectHoveringImages();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startLocation = transform.position;
        image.color = dragging;

        thisItemBeingDragged = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (MouseOverDiscard()) 
        {
            inv.DiscardItemFromInventory(itemID, item);
            Destroy(gameObject);
        }else 
        {
            // Color the images depending on item size, green - good loc, red - bad loc
            // else if the item is in new viable location, change the items location in the inventory
            // RemoveItem (itemID)
            // PlaceItem (Item, itemID)
            transform.position = startLocation;
            image.color = startColor;

            if (item.sizeX != startItemX && item.sizeY != startItemY)
            {
                RotateItem();
            }        
        }

        inv.ResetAllSelectedImages();
    }

    private void RotateItem ()
    {
        Sprite tempSprite = item.sprite;
        item.sprite = item.rotatedSprite;
        item.rotatedSprite = tempSprite;

        int tempSizeX = item.sizeX;
        item.sizeX = item.sizeY;
        item.sizeY = tempSizeX;

        image.sprite = item.sprite;
        image.SetNativeSize();
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
