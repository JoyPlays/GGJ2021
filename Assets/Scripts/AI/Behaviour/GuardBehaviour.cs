using System;
using Pathfinding;
using UnityEngine;

public class GuardBehaviour : AIBehaviour
{
	[SerializeField] private AIPath ai;
	
	[Space]
	[SerializeField] private Transform guardPoint;
	
	public override AIPath AI => ai;

	private void Start()
	{
		MoveToGuardPoint();
	}

	private void Update()
	{
		if (!Enabled)
		{
			return;
		}
		
		if (AI.reachedEndOfPath && !AI.pathPending)
		{
			ai.transform.rotation = Quaternion.LookRotation(guardPoint.forward);
		}
	}

	private void MoveToGuardPoint()
	{
		AI.destination = guardPoint.position;

		AI.SearchPath();
	}

	protected override void OnBehaviourEnabled()
	{
		MoveToGuardPoint();
	}

	protected override void OnBehaviourDisabled()
	{
		AI.SetPath(null);
		AI.destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
	}
}
