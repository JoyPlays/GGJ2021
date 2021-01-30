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

	[SerializeField] private float agroRadius = 5f;

	[SerializeField] private LayerMask agroLayerMask;

	public bool IsMoving { get; set; } = false;
	public HealthController HealthController => healthController;

	public Animator Animator => animator;

	private void Start()
	{
		agroBehaviour.Enabled = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			TakeDamage(50f);
		}
		
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

	
	public void TakeDamage(float damageAmount)
	{
		if (healthController)
		{
			healthController.TakeDamage(damageAmount);
		}
		
		DamageResponse();
	}

	public void DamageResponse()
	{
		Debug.Log("Damage Response");
	}

	public void Die()
	{
		ai.destination = transform.position;
		agroBehaviour.Enabled = false;
		aiBehaviour.Enabled = false;
		Animator.SetTrigger("Death");
	}
}

