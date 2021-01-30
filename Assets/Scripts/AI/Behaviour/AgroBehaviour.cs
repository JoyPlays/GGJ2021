using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AgroBehaviour : AIBehaviour
{
	[SerializeField] private AIPath ai;
	[SerializeField] private EnemyAI enemyAI;

	[SerializeField] private float resetAgroDistance = 15f;
	
	private Transform agroTarget;

	public override AIPath AI => ai;
	
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

		if (Vector3.Distance(transform.position, agroTarget.position) >= resetAgroDistance)
		{
			Enabled = false;
		}
		else
		{
			AI.destination = agroTarget.position;
		}
	}
	
	protected override void OnBehaviourEnabled()
	{
		AI.destination = agroTarget.position;
		AI.SearchPath();
	}

	protected override void OnBehaviourDisabled()
	{
		agroTarget = null;
		enemyAI.AgroOutOfRange();
	}
}
