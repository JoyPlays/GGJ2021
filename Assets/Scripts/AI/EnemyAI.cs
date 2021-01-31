using System;
using Pathfinding;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IDamageable
{
	[SerializeField] private Animator animator;
	[SerializeField] private AIPath ai;
	[SerializeField] private AIBehaviour aiBehaviour;
	[SerializeField] private AgroBehaviour agroBehaviour;
	[SerializeField] private HealthController healthController;
	[SerializeField] private Collider collider;
	[SerializeField] private GameObject bloodParticle;

	[SerializeField] private float agroRadius = 5f;

	[SerializeField] private LayerMask agroLayerMask;

	private GameObject[] bloodPool = new GameObject[5];
	
	public bool IsMoving { get; set; } = false;
	public HealthController HealthController => healthController;

	public Animator Animator => animator;

	private void Awake()
	{
		if (bloodParticle)
		{
			for (int i = 0; i < bloodPool.Length; i++)
			{
				bloodPool[i] = Instantiate(bloodParticle);
				bloodPool[i].gameObject.SetActive(false);
			}
		}
	}
	
	private void Start()
	{
		agroBehaviour.Enabled = false;
	}

	private void Update()
	{
		bool isMoving = aiBehaviour.GetSpeedPercent() > 0.1f;
		Animator.SetBool("isRunning", isMoving);
		
		if (!agroBehaviour.Enabled && healthController.Alive)
		{
			Collider[] agroResult = new Collider[10];
			Physics.OverlapSphereNonAlloc(transform.position, agroRadius, agroResult, agroLayerMask);

			foreach (var agroTarget in agroResult)
			{
				if (agroTarget)
				{
					aiBehaviour.Enabled = false;
					agroBehaviour.SetupAgroTarget(agroTarget.transform);
					agroBehaviour.Enabled = true;
					break;
				}
			}
		}
	}
	
	public void AgroOutOfRange()
	{
		aiBehaviour.Enabled = true;
	}

	private void OnDrawGizmos()
	{
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		Gizmos.color = new Color(0.75f, 0, 0, 0.1f);
		Gizmos.DrawSphere(Vector3.zero, agroRadius / 2);
	}

	
	public void TakeDamage(float damageAmount, Vector3 hitPos)
	{
		if (healthController)
		{
			healthController.TakeDamage(damageAmount);
		}
		
		DamageResponse(hitPos);
	}

	public void DamageResponse(Vector3 hitPos)
	{
		GameObject bloodfx = null;

		for (int i = 0; i < bloodPool.Length; i++)
		{
			if (!bloodPool[i].activeInHierarchy)
			{
				bloodfx = bloodPool[i];
				break;
			}
		}

		if (!bloodfx)
		{
			bloodfx = bloodPool[0];
		}
		bloodfx.SetActive(false);
		bloodfx.transform.position = hitPos;
		bloodfx.SetActive(true);
	}

	public void Die()
	{
		collider.enabled = false;
		ai.destination = transform.position;
		agroBehaviour.Enabled = false;
		aiBehaviour.Enabled = false;
		Animator.SetTrigger("Death");
	}
}

