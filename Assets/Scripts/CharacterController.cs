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
    [SerializeField] private HealthController healthController;
	
	[SerializeField] private Transform shootPoint;
	[SerializeField] private GameObject fakeProjectile;

    [Header("Params")]
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float boxCastDistance = 1f;
    [SerializeField] private Vector3 sphereCastOffset = Vector3.up;
	
	[SerializeField] private float delayBetweenShots = 2f;
	
	
	private GameObject[] fakeProjectilePool = new GameObject[20];
	
	private bool canSHoot = true;
	private float nextShotTime;

    private float characterAngle = 0;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 1));
    }
	
	private void Awake()
	{
		if (fakeProjectile)
		{
			for (int i = 0; i < fakeProjectilePool.Length; i++)
			{
				fakeProjectilePool[i] = Instantiate(fakeProjectile);
				fakeProjectilePool[i].gameObject.SetActive(false);
			}
		}
	}

    void Update()
    {
		if (healthController && !healthController.Alive)
		{
			return;
		}
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
			if (canSHoot && Time.time >= nextShotTime)
			{
				canSHoot = false;
				Shooting();
			}
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

		/*
        else if (Input.GetKey(KeyCode.S))
        {
            isRunning = true;
            nextPosition = transform.position - (Time.deltaTime * finalSpeed) * transform.forward;
        }
		*/

        Collider[] colliders = Physics.OverlapSphere(nextPosition + sphereCastOffset, 0.5f, wallLayer, QueryTriggerInteraction.Ignore);

        if (colliders.Length <= 0)
        {
            transform.position = nextPosition;
        }

		/*
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
		*/
        animator.SetBool("isRunning", isRunning);
    }

	private void Shooting()
	{
		//Vector3 targetShootPos = transform.position + transform.forward;
		//targetShootPos.y = 2.5f;
		Vector3 shootDirection = transform.forward;
		
		RaycastHit hit;
		Ray shootRay = new Ray(shootPoint.position, shootDirection);
		bool hitSomething = Physics.Raycast(shootRay, out hit, 50f);
		Debug.DrawRay(shootPoint.position, shootDirection * 50f, Color.cyan);

		Vector3 projectileEndPos = shootPoint.position + (shootDirection * 50f);
		if (hitSomething)
		{
			if (hit.transform.gameObject.layer == 12)
			{
				IDamageable damageable = hit.transform.gameObject.GetComponent<IDamageable>();
				damageable.TakeDamage(20f);
			}
			projectileEndPos = hit.point;
		}

		if (fakeProjectile)
		{
			_ = StartCoroutine(LaunchFakeProjectile(shootPoint, projectileEndPos));
		}
		
		canSHoot = true;
		nextShotTime = Time.time + delayBetweenShots;
	}
	
	private IEnumerator LaunchFakeProjectile(Transform startPoint, Vector3 endPos)
	{
		GameObject projectile = null;

		for (int i = 0; i < fakeProjectilePool.Length; i++)
		{
			if (!fakeProjectilePool[i].activeInHierarchy)
			{
				projectile = fakeProjectilePool[i];
				break;
			}
		}

		if (!projectile)
		{
			projectile = fakeProjectilePool[0];
		}
		
		projectile.transform.position = startPoint.position;
		projectile.SetActive(true);

		float dist = Vector3.Distance(startPoint.position, endPos);

		float t = 0f;

		while (t < 1f)
		{
			t += Time.deltaTime / (0.01f *  dist);

			projectile.transform.position = Vector3.Lerp(startPoint.position, endPos, t);
			
			yield return null;
		}
		projectile.SetActive(false);
	}
}
