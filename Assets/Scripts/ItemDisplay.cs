using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    public Item item;

    [SerializeField] Collider coll = null;
    [SerializeField] MeshFilter meshFilter = null;

    void Start()
    {
        EnableMesh();
    }

    public void EnableMesh()
    {
        coll.enabled = true;
        meshFilter.sharedMesh = item.worldObj;
    }

    public void DisableMesh () {
        coll.enabled = false;
        meshFilter.sharedMesh = null;
    }
}
