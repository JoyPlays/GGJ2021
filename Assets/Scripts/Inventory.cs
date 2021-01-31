using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int colSize = 0;
    public int rowSize = 0;

    public int[,] inv;

    public List<int> slotX = new List<int>();
    public List<int> slotY = new List<int>();


    [SerializeField] Canvas canvas = null;

    [SerializeField] GameObject invSlotPrefab = null;
    [SerializeField] GameObject itemIconPrefab = null;

    [SerializeField] float gridCellSize = 0;
    [SerializeField] GameObject inventoryPanel = null;
    [SerializeField] GridLayoutGroup gridLayoutGroup = null;

    [SerializeField] RectTransform discardPanel = null;

    private int globalItemID = 1;
    private List<Image> invImages = new List<Image>();
    private Dictionary<int, GameObject> createdItemIcons = new Dictionary<int, GameObject>();

    private void Awake()
    {
        CreateInventorySlots();
        canvas.enabled = false;
    }

    private void Start()
    {
        inv = new int[colSize, rowSize];

        for (int x = 0; x < colSize; x++)
        {
            for (int y = 0; y < rowSize; y++)
            {
                inv[x, y] = 0;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            if (canvas.isActiveAndEnabled)
            {
                canvas.enabled = false;
            }
            else 
            {
                canvas.enabled = true;
            }
        }   
    }

    public void PlaceItemInInventoryFromPickup(Item item, GameObject itemPrefab)
    {
        List<int> freeX = new List<int>();
        List<int> freeY = new List<int>();

        int newSizeX = item.sizeX;
        int newSizeY = item.sizeY;

        for (int x = 0; x < colSize; x++)
        {
            for (int y = 0; y < rowSize; y++)
            {
                int checkIfRotated = 0;

                CheckRotated:
                bool foundPlaceForItem = true;

                for (int nextX = 0; nextX < newSizeX; nextX++)
                {
                    for (int nextY = 0; nextY < newSizeY; nextY++)
                    {
                        if (x + nextX >= colSize || y + nextY >= rowSize || inv[x + nextX, y + nextY] != 0)
                        {
                            foundPlaceForItem = false;
                            if (checkIfRotated == 0 && newSizeX != newSizeY) // MAKE MORE PERFORMANT
                            {
                                checkIfRotated++;
                            }
                            goto BreakFrom2For;
                        }
                        else
                        {
                            freeX.Add(x + nextX);
                            freeY.Add(y + nextY);
                        }
                    }
                }

                BreakFrom2For:
                if (foundPlaceForItem)
                {
                    for (int z = 0; z < freeX.Count; z++)
                    {
                        inv[freeX[z], freeY[z]] = globalItemID;
                        invImages[freeX[z] + freeY[z] + ((colSize - 1) * freeY[z])].GetComponent<InventorySlot>().isEmpty = false;
                    }

                    Vector2 centerPos = FindCenterFromImages(freeX, freeY);

                    PlaceItemImage(centerPos.x, centerPos.y, newSizeX, newSizeY, item, itemPrefab, globalItemID);

                    return;
                }
                else
                {
                    freeX.Clear();
                    freeY.Clear();

                    if (checkIfRotated == 1)
                    {
                        int tempX = newSizeX;
                        newSizeX = newSizeY;
                        newSizeY = tempX;

                        checkIfRotated++;

                        goto CheckRotated;
                    }
                }
            }
        }
    }

    private Vector2 FindCenterFromImages (List<int> arrayWithSlotX, List<int> arrayWithSlotY) 
    {
        int firstIndex = arrayWithSlotX[0] + arrayWithSlotY[0] + ((colSize - 1) * arrayWithSlotY[0]);
        int lastIndex = arrayWithSlotX[arrayWithSlotX.Count - 1] + arrayWithSlotY[arrayWithSlotX.Count - 1] + ((colSize - 1) * arrayWithSlotY[arrayWithSlotX.Count - 1]);

        float centerX = invImages[lastIndex].rectTransform.anchoredPosition.x
                      - invImages[firstIndex].rectTransform.anchoredPosition.x;

        if (centerX == 0)
        {
            centerX = invImages[lastIndex].rectTransform.anchoredPosition.x;
        }
        else
        {
            centerX += gridCellSize * arrayWithSlotX[0];
        }

        float centerY = invImages[lastIndex].rectTransform.anchoredPosition.y
                      - invImages[firstIndex].rectTransform.anchoredPosition.y;

        if (centerY == 0)
        {
            centerY = invImages[lastIndex].rectTransform.anchoredPosition.y;
        }
        else
        {
            centerY -= gridCellSize * arrayWithSlotY[0];
        }

        return new Vector2(centerX, centerY);
    }

    private void PlaceItemImage(float xPos, float yPos, int newSizeX, int newSizeY, Item item, GameObject itemPrefab, int newItemID)
    {
        GameObject itemIcon = Instantiate(itemIconPrefab, inventoryPanel.transform);

        RectTransform trans = itemIcon.GetComponent<RectTransform>();
        trans.anchoredPosition = new Vector2(xPos, yPos);

        Image image = itemIcon.GetComponent<Image>();

        if (newSizeX != item.sizeX && newSizeY != item.sizeY)
        {
            trans.rotation = Quaternion.Euler(0f, 0f, 90f);
        }

        image.sprite = item.sprite;
        image.SetNativeSize();

        InventoryItem invItem = itemIcon.GetComponent<InventoryItem>();
        invItem.inv = this;
        invItem.discardPanel = discardPanel;
        invItem.item = item;
        invItem.itemID = newItemID;
        invItem.itemPrefab = itemPrefab;

        createdItemIcons.Add(globalItemID, itemIcon);

        globalItemID++;
    }

    public void DiscardItemFromInventory (int itemID, GameObject itemPrefab) 
    {
        RemoveItemFromInventory(itemID);
        CreateItemObjectIn3D(itemPrefab);
    }

    public void RemoveItemFromInventory (int itemID) 
    {
        for (int x = 0; x < colSize; x++)
        {
            for (int y = 0; y < rowSize; y++)
            {
                if (inv[x, y] == itemID)
                {
                    inv[x, y] = 0;
                    int invImageIndex = x + y + (colSize - 1) * y;
                    invImages[invImageIndex].GetComponent<InventorySlot>().isEmpty = true;
                }
            }
        }
    }

    private void CreateItemObjectIn3D(GameObject itemPrefab)
    {
        itemPrefab.transform.position = gameObject.transform.position;
        itemPrefab.SetActive(true);   
    }

    public void SelectHoveringImages (int sizeX, int sizeY, int newItemID) 
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        for (int x = 0; x < results.Count; x++)
        {
            InventorySlot invSlot = results[x].gameObject.GetComponent<InventorySlot>();

            if (invSlot)
            {
                for (int y = 0; y < invImages.Count; y++) 
                {
                    InventorySlot currentInvSlot = invImages[y].GetComponent<InventorySlot>();

                    if (invSlot == currentInvSlot)
                    {
                        FindCorrectImagesToHighlight(y, sizeX, sizeY, newItemID);
                    }
                }
            }
        }
    }

    private void FindCorrectImagesToHighlight (int imageLocInArray, int sizeX, int sizeY, int newItemID) 
    {
        slotX.Clear();
        slotY.Clear();

        for (int x = 0; x < colSize; x++) 
        {
            for (int y = 0; y < rowSize; y++) 
            {
                int startPoint = imageLocInArray - (colSize - 1) * y;

                if (startPoint == x + y)
                {
                    for (int nextX = 0; nextX < sizeX; nextX++)
                    {
                        for (int nextY = 0; nextY < sizeY; nextY++)
                        {
                            if (x + nextX < colSize && y + nextY < rowSize)
                            {
                                slotX.Add(x + nextX);
                                slotY.Add(y + nextY);
                            }
                        }
                    }

                    for (int z = 0; z < invImages.Count; z++)
                    {             
                        InventorySlot randomImage = invImages[z].GetComponent<InventorySlot>();
                        randomImage.ObjectNotOverInventorySlot();
                    }

                    InventoryItem itemBeingDragged = createdItemIcons[newItemID].GetComponent<InventoryItem>();
                    itemBeingDragged.foundPlaceForItem = true;

                    for (int g = 0; g < slotX.Count; g++)
                    {
                        InventorySlot slot = invImages[slotX[g] + slotY[g] + ((colSize - 1) * slotY[g])].GetComponent<InventorySlot>();
                        slot.ObjectOverInventorySlot();

                        if (!slot.isEmpty)
                        {
                            itemBeingDragged.foundPlaceForItem = false;
                        }
                    }

                    return;
                }
            }
        }
    }

    public void MoveItemToNewLocation (int sizeX, int sizeY, int newItemID) 
    {
        if (slotX.Count == sizeX * sizeY)
        {
            RemoveItemFromInventory(newItemID);

            for (int g = 0; g < slotX.Count; g++)
            {
                inv[slotX[g], slotY[g]] = newItemID;
                invImages[slotX[g] + slotY[g] + ((colSize - 1) * slotY[g])].GetComponent<InventorySlot>().isEmpty = false;
            }

            Vector2 centerOfSlots = FindCenterFromImages(slotX, slotY);

            createdItemIcons[newItemID].GetComponent<RectTransform>().anchoredPosition = centerOfSlots;
        }
    }

    public void ResetAllSelectedImages () 
    {
        for (int x = 0; x < invImages.Count; x++)
        {
            InventorySlot currentInvSlot = invImages[x].GetComponent<InventorySlot>();
            currentInvSlot.ObjectNotOverInventorySlot();
        }
    }

    private void CreateInventorySlots () 
    {
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = colSize;
        gridLayoutGroup.cellSize = new Vector2(gridCellSize, gridCellSize);

        for (int x = 0; x < rowSize * colSize; x++)
        {
            GameObject newImage = Instantiate(invSlotPrefab, gridLayoutGroup.transform);

            newImage.GetComponent<InventorySlot>().inv = this;

            invImages.Add(newImage.GetComponent<Image>());
        }
    }
}
