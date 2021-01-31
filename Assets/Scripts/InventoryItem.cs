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
    public GameObject itemPrefab;
    public bool foundPlaceForItem = true;

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
            inv.SelectHoveringImages(startItemX, startItemY, itemID);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;

        inv.SelectHoveringImages(startItemX, startItemY, itemID);
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
            inv.DiscardItemFromInventory(itemID, itemPrefab);
            Destroy(gameObject);
        }else 
        {
            if (foundPlaceForItem)
            {
                inv.MoveItemToNewLocation(startItemX, startItemY, itemID);
            }
            else
            {
                if (item.sizeX != startItemX && item.sizeY != startItemY)
                {
                    RotateItem();
                }

                transform.position = startLocation;
            }

            image.color = startColor;
        }

        thisItemBeingDragged = false;
        inv.ResetAllSelectedImages();
    }

    private void RotateItem()
    {
        if (transform.rotation == Quaternion.Euler(0f, 0f, 90f) && thisItemBeingDragged) 
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else 
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }

        int tempSizeX = startItemX;
        startItemX = startItemY;
        startItemY = tempSizeX;
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
