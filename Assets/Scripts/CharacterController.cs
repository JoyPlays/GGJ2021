using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private new Camera camera;
    [SerializeField] private Animator animator;

    [Header("Params")]
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float sideSpeed = 1f;
    [SerializeField] private float rotationSpeed = 1f;

    private float characterAngle = 0;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 1));
    }

    void Update()
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

        if (Input.GetKey(KeyCode.W))
        {
            isRunning = true;
            transform.position += (Time.deltaTime * finalSpeed) * transform.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            isRunning = true;
            transform.position -= (Time.deltaTime * finalSpeed) * transform.forward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            isRunning = true;
            transform.position += (Time.deltaTime * sideSpeed) * transform.right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            isRunning = true;
            transform.position -= (Time.deltaTime * sideSpeed) * transform.right;
        }

        animator.SetBool("isRunning", isRunning);

        if (Input.GetMouseButton(0))
        {
            animator.SetTrigger("Shoot");
        }
    }
}
