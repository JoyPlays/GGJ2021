using System.Collections.Generic;
using UnityEngine;

public class CameraXray : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform character;

    [Header("Params")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Material transparent;
    [SerializeField] private Material baseMaterial;
    [SerializeField, Range(0, 1)] private float transparencyAmount = 0.1f;

    private List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

    void Update()
    {
        XRay();
    }

    private void XRay()
    {
        float characterDistance = Vector3.Distance(transform.position, character.position);

        Vector3 direction = (character.position - transform.position).normalized;

        RaycastHit[] raycasts = Physics.RaycastAll(transform.position, direction, characterDistance);

        List<MeshRenderer> gatheredMeshRenderers = new List<MeshRenderer>();

        foreach (var item in raycasts)
        {
            if (item.transform.gameObject == character.gameObject)
            {
                continue;
            }

            MeshRenderer meshRenderer = item.transform.gameObject.GetComponent<MeshRenderer>();

            if (!meshRenderer)
            {
                continue;
            }

            gatheredMeshRenderers.Add(meshRenderer);

            meshRenderer.material = transparent;

            Color color = Color.white;

            color.a = transparencyAmount;

            meshRenderer.material.SetColor("_BaseColor", color);
        }

        foreach (var item in meshRenderers)
        {
            if (!gatheredMeshRenderers.Contains(item))
            {
                item.material = baseMaterial;
            }
        }

        meshRenderers = gatheredMeshRenderers;
    }
}
