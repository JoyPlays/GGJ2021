using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    public Item item;

    [SerializeField] Collider itemMeshCollider = null;
    [SerializeField] MeshFilter itemMeshFilter = null;

    public void DisableObject () 
    {
        gameObject.SetActive(false);
    }
}
