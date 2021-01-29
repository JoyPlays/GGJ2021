using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int colSize = 0;
    public int rowSize = 0;

    public int[,] inv;

    void Start()
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

    public void PlaceItemInInventory(int xSize, int ySize)
    {
        List<int> freeX = new List<int>();
        List<int> freeY = new List<int>();

        for (int x = 0; x < colSize; x++)
        {
            for (int y = 0; y < rowSize; y++)
            {
                int checkIfRotated = 0;

            CheckRotated:
                bool foundPlaceForItem = true;

                for (int nextX = 0; nextX < xSize; nextX++)
                {
                    for (int nextY = 0; nextY < ySize; nextY++)
                    {
                        if (x + nextX >= colSize || y + nextY >= rowSize || inv[x + nextX, y + nextY] != 0)
                        {
                            foundPlaceForItem = false;
                            if (checkIfRotated == 0)
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
                        inv[freeX[z], freeY[z]] = 1;
                    }

                    return;
                }
                else
                {
                    freeX.Clear();
                    freeY.Clear();

                    if (checkIfRotated == 1)
                    {
                        int tempX = xSize;
                        xSize = ySize;
                        ySize = tempX;

                        checkIfRotated++;

                        goto CheckRotated;
                    }
                }
            }
        }
    }
}
