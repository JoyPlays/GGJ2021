using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public enum type
    {
        Weapon,
        Helmet,
        Armor,
        Backpack,
        Food
    };

    public type itemType;

    public new string name;
    public string desc;

    public int value;
    public int sizeX;
    public int sizeY;

    public Sprite sprite;
}
