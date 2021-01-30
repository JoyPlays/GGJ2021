using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AgroBehaviour : AIBehaviour
{
	[SerializeField] private AIPath ai;
	[SerializeField] private EnemyAI enemyAI;

	[SerializeField] private Transform shootPoint;
	[SerializeField] private GameObject fakeProjectile;

	[SerializeField] private float resetAgroDistance = 15f;
	[SerializeField] private float shootRange = 20f;
	[SerializeField] private float speedWhenShooting = 2f;
	[SerializeField] private float delayBetweenShots = 2f;

	private GameObject[] fakeProjectilePool = new GameObject[3];
	
	private bool canSHoot = true;
	private float nextShotTime;
	
	private Transform agroTarget;
	private float moveSpeed;

	public override AIPath AI => ai;

	private void Awake()
	{
		moveSpeed = AI.maxSpeed;

		if (fakeProjectile)
		{
			for (int i = 0; i < fakeProjectilePool.Length; i++)
			{
				fakeProjectilePool[i] = Instantiate(fakeProjectile);
				fakeProjectilePool[i].gameObject.SetActive(false);
			}
		}
	}

	public void SetupAgroTarget(Transform target)
	{
		agroTarget = target;
	}

	public void Update()
	{
		if (!Enabled || !agroTarget)
		{
			return;
		}

		float distance = Vector3.Distance(transform.position, agroTarget.position);
		
		if (distance >= resetAgroDistance)
		{
			Enabled = false;
		}
		else
		{
			if (canSHoot && distance < shootRange && Time.time >= nextShotTime)
			{
				canSHoot = false;
				_ = StartCoroutine(ShootSequence());
			}
			
			AI.destination = agroTarget.position;
		}
	}

	private IEnumerator ShootSequence()
	{
		ai.maxSpeed = speedWhenShooting;

		enemyAI.Animator.SetTrigger("Shoot");

		Vector3 targetShootPos = agroTarget.position;
		targetShootPos.y = 2.5f;
		Vector3 shootDirection = transform.forward;
		
		for (int i = 0; i < 3; i++)
		{
			RaycastHit hit;
			Ray shootRay = new Ray(shootPoint.position, shootDirection);
			bool hitSomething = Physics.Raycast(shootRay, out hit, 15f);
			Debug.DrawRay(shootPoint.position, shootDirection * 15f, Color.red);

			Vector3 projectileEndPos = shootPoint.position + (shootDirection * 15f);
			if (hitSomething)
			{
				if (hit.transform.gameObject.layer == 11)
				{
					Debug.Log("GOT PLAYER SHOT");
				}
				projectileEndPos = hit.point;
			}

			if (fakeProjectile)
			{
				_ = StartCoroutine(LaunchFakeProjectile(i, shootPoint, projectileEndPos));
			}
			
			yield return new WaitForSeconds(0.333f);
		}
		
		canSHoot = true;
		nextShotTime = Time.time + delayBetweenShots;
	}

	private IEnumerator LaunchFakeProjectile(int projectileIndex, Transform startPoint, Vector3 endPos)
	{
		GameObject projectile = fakeProjectilePool[projectileIndex].gameObject;
		projectile.transform.position = startPoint.position;
		projectile.SetActive(true);
		

		float t = 0f;

		while (t < 1f)
		{
			t += Time.deltaTime / 0.1f;

			projectile.transform.position = Vector3.Lerp(startPoint.position, endPos, t);
			
			yield return null;
		}
		
		projectile.SetActive(false);
	}

	protected override void OnBehaviourEnabled()
	{
		nextShotTime = Time.time + delayBetweenShots;
		AI.endReachedDistance = 6;
		AI.destination = agroTarget.position;
		AI.SearchPath();
	}

	protected override void OnBehaviourDisabled()
	{
		AI.endReachedDistance = 1;
		agroTarget = null;
		enemyAI.AgroOutOfRange();
	}
}
