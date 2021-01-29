using System;
using Pathfinding;
using UnityEngine;

public class GuardBehaviour : AIBehaviour
{
	[SerializeField] private AIPath ai;
	
	[Space]
	[SerializeField] private Transform guardPoint;

	private void Start()
	{
		MoveToGuardPoint();
	}

	private void MoveToGuardPoint()
	{
		ai.destination = guardPoint.position;

		ai.SearchPath();
	}

	protected override void OnBehaviourEnabled()
	{
		MoveToGuardPoint();
	}

	protected override void OnBehaviourDisabled()
	{
		ai.SetPath(null);
		ai.destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
	}
}
