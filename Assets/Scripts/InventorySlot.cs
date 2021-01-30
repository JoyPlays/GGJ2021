using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Inventory inv = null;
    public bool isEmpty = true;

    [SerializeField] Image image = null; 

    public void ObjectOverInventorySlot () 
    {
        if (isEmpty)
        {
            image.color = new Color(0f, 1f, 0f, 1f);
        }
        else
        {
            image.color = new Color(1f, 0f, 0f, 1f);
        }
    }

    public void ObjectNotOverInventorySlot()
    {
        image.color = new Color(255f, 255f, 255f, 1f);
    }
}
