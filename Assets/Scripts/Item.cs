using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public new string name;
    public string desc;

    public int value;
    public int sizeX;
    public int sizeY;

    public Sprite invIcon;
    public Mesh worldObj;
}
