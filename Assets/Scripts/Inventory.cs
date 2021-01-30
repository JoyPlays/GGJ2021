using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int colSize = 0;
    public int rowSize = 0;

    public int[,] inv;

    [SerializeField] GameObject invSlotPrefab = null;
    [SerializeField] GameObject itemIconPrefab = null;
    [SerializeField] GameObject item3DPrefab = null;

    [SerializeField] float gridCellSize = 0;
    [SerializeField] GameObject gridPanel = null;
    [SerializeField] GridLayoutGroup gridLayoutGroup = null;

    [SerializeField] RectTransform discardPanel = null;

    private int itemID = 1;
    private List<Image> invImages = new List<Image>();

    private void Awake()
    {
        gridLayoutGroup.cellSize = new Vector2(gridCellSize, gridCellSize);
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

        CreateInventorySlots();
    }

    public void PlaceItemInInventory(Item item)
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
                        inv[freeX[z], freeY[z]] = itemID;
                    }

                    int firstIndex = freeX[0] + freeY[0] + ((colSize - 1) * freeY[0]);
                    int lastIndex = freeX[freeX.Count - 1] + freeY[freeX.Count - 1] + ((colSize - 1) * freeY[freeX.Count - 1]);

                    float centerX = invImages[lastIndex].rectTransform.anchoredPosition.x
                                  - invImages[firstIndex].rectTransform.anchoredPosition.x;

                    if (centerX == 0)
                    {
                        centerX = invImages[lastIndex].rectTransform.anchoredPosition.x;
                    }
                    else
                    {
                        centerX += gridCellSize * freeX[0];
                    }

                    float centerY = invImages[lastIndex].rectTransform.anchoredPosition.y
                                  - invImages[firstIndex].rectTransform.anchoredPosition.y;

                    if (centerY == 0)
                    {
                        centerY = invImages[lastIndex].rectTransform.anchoredPosition.y;
                    }
                    else
                    {
                        centerY -= gridCellSize * freeY[0];
                    }

                    PlaceItemImage(centerX, centerY, newSizeX, newSizeY, item, itemID);

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

    public void RemoveItemFromInventory (int itemID, Item item) 
    {
        for (int x = 0; x < colSize; x++)
        {
            for (int y = 0; y < rowSize; y++)
            {
                if (inv[x, y] == itemID)
                {
                    inv[x, y] = 0;
                }
            }
        }

        CreateItemObjectIn3D(item);
    }

    private void CreateItemObjectIn3D (Item item) 
    {
        GameObject item3D = Instantiate(item3DPrefab);

        item3D.GetComponent<ItemDisplay>().item = item;
    }

    private void PlaceItemImage (float xPos, float yPos, int newSizeX, int newSizeY, Item item, int newItemID)
    {
        GameObject itemIcon = Instantiate(itemIconPrefab, gridPanel.transform);

        RectTransform trans = itemIcon.GetComponent<RectTransform>();
        trans.anchoredPosition = new Vector2(xPos, yPos);
        
        Image image = itemIcon.GetComponent<Image>();

        InventoryItem invItem = itemIcon.GetComponent<InventoryItem>();
        invItem.inv = this;
        invItem.discardPanel = discardPanel;
        invItem.item = item;
        invItem.itemID = newItemID;
        itemID++;

        if (newSizeX != item.sizeX && newSizeY != item.sizeY)
        {
            Sprite tempSprite = item.sprite;
            item.sprite = item.rotatedSprite;
            item.rotatedSprite = tempSprite;

            image.sprite = item.sprite;

            item.sizeX = newSizeX;
            item.sizeY = newSizeY;
        }else
        {
            image.sprite = item.sprite;
        }

        image.SetNativeSize();
    }

    private void CreateInventorySlots () 
    {
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = colSize;

        for (int x = 0; x < rowSize * colSize; x++)
        {
            GameObject newImage = Instantiate(invSlotPrefab, gridLayoutGroup.transform);

            invImages.Add(newImage.GetComponent<Image>());
        }
    }
}
