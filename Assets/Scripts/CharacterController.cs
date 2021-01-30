using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask wallLayer;

    [Header("Components")]
    [SerializeField] private new Camera camera;
    [SerializeField] private Animator animator;

    [Header("Params")]
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float boxCastDistance = 1f;
    [SerializeField] private Vector3 sphereCastOffset = Vector3.up;

    private float characterAngle = 0;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 1));
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float finalSpeed = movementSpeed;

        Ray cameraRay = camera.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(cameraRay, out float rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            finalSpeed *= 2;
        }

        bool isRunning = false;

        Vector3 nextPosition = transform.position;

        if (Input.GetMouseButton(0))
        {
            animator.SetBool("Shoot", true);
            finalSpeed = movementSpeed / 2;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("Shoot", false);
        }

        if (Input.GetKey(KeyCode.W))
        {
            isRunning = true;
            nextPosition = transform.position + (Time.deltaTime * finalSpeed) * transform.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            isRunning = true;
            nextPosition = transform.position - (Time.deltaTime * finalSpeed) * transform.forward;
        }

        Collider[] colliders = Physics.OverlapSphere(nextPosition + sphereCastOffset, 0.5f, wallLayer, QueryTriggerInteraction.Ignore);

        if (colliders.Length <= 0)
        {
            transform.position = nextPosition;
        }

        if (Input.GetKey(KeyCode.D))
        {
            isRunning = true;
            nextPosition = transform.position + (Time.deltaTime * finalSpeed) * transform.right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            isRunning = true;
            nextPosition = transform.position - (Time.deltaTime * finalSpeed) * transform.right;
        }

        colliders = Physics.OverlapSphere(nextPosition + sphereCastOffset, 0.5f, wallLayer, QueryTriggerInteraction.Ignore);

        if (colliders.Length <= 0)
        {
            transform.position = nextPosition;
        }

        animator.SetBool("isRunning", isRunning);
    }
}
