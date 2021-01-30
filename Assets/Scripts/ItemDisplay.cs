using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    public Item item;

    [SerializeField] Collider itemMeshCollider = null;
    [SerializeField] MeshFilter itemMeshFilter = null;

    void Start()
    {
        EnableMesh();
    }

    public void EnableMesh()
    {
        itemMeshCollider.enabled = true;
        itemMeshFilter.sharedMesh = item.worldObj;
    }

    public void DisableMesh () 
    {
        itemMeshCollider.enabled = false;
        itemMeshFilter.sharedMesh = null;
    }
}
