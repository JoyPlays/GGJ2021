using UnityEngine;

public class CameraXray : MonoBehaviour
{
    [SerializeField] private Material transparent;
    [SerializeField] private Material baseMaterial;
    [SerializeField] private Transform character;
    [SerializeField] private LayerMask layerMask;

    private MeshRenderer meshRenderer;

    void Update()
    {
        XRay();
    }

    private void XRay()
    {
        float characterDistance = Vector3.Distance(transform.position, character.position);

        Vector3 direction = (character.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, characterDistance))
        {
            if (hit.collider.gameObject == character.gameObject)
            {
                if (meshRenderer)
                {
                    meshRenderer.material = baseMaterial;
                }

                return;
            }

            if (hit.transform.gameObject.GetComponent<MeshRenderer>() != meshRenderer)
            {
                if (meshRenderer)
                {
                    meshRenderer.material = baseMaterial;
                }

                meshRenderer = hit.transform.gameObject.GetComponent<MeshRenderer>();
            }

            meshRenderer.material = transparent;

            Color color = Color.white;

            color.a = 0.25f;

            hit.transform.gameObject.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", color);
        }
    }
}
