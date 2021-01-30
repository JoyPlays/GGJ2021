using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] Inventory inv = null;

    private List<ItemDisplay> itemInRange = new List<ItemDisplay>();
    private int layerMask;

    void Start()
    {
        layerMask = LayerMask.GetMask("Item");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    ItemDisplay clickedObjDisplay = hit.transform.gameObject.GetComponent<ItemDisplay>();

                    if (clickedObjDisplay)
                    {
                        for (int x = 0; x < itemInRange.Count; x++)
                        {
                            if (itemInRange[x] == clickedObjDisplay)
                            {
                                clickedObjDisplay.DisableMesh();
                                inv.PlaceItemInInventoryFromPickup(clickedObjDisplay.item);

                                break;
                            }
                        }
                    }
                }     
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        itemInRange.Add(other.gameObject.GetComponent<ItemDisplay>());
    }

    private void OnTriggerExit(Collider other)
    {
        itemInRange.Remove(other.gameObject.GetComponent<ItemDisplay>());
    }
}
