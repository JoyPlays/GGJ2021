using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public new string name;
    public string desc;

    public int value;
    public int sizeX;
    public int sizeY;

    public Sprite rotatedSprite;
    public Sprite sprite;
    public Mesh worldObj;
}
