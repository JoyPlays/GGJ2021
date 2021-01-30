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
    [SerializeField] private float sideSpeed = 1f;
    [SerializeField] private float boxCastDistance = 1f;
    [SerializeField] private Vector3 boxCastSize = Vector3.one;
    [SerializeField] private Vector3 boxCastOffset = Vector3.up;

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

        if (!Physics.BoxCast(nextPosition + boxCastOffset, boxCastSize, Vector3.forward, Quaternion.identity, boxCastDistance, wallLayer))
        {
            transform.position = nextPosition;
        }

        if (Input.GetKey(KeyCode.D))
        {
            isRunning = true;
            nextPosition = transform.position + (Time.deltaTime * sideSpeed) * transform.right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            isRunning = true;
            nextPosition = transform.position - (Time.deltaTime * sideSpeed) * transform.right;
        }

        if (!Physics.BoxCast(nextPosition + boxCastOffset, boxCastSize, Vector3.forward, Quaternion.identity, boxCastDistance, wallLayer))
        {
            transform.position = nextPosition;
        }

        animator.SetBool("isRunning", isRunning);

        if (Input.GetMouseButton(0))
        {
            animator.SetTrigger("Shoot");
        }
    }
}
