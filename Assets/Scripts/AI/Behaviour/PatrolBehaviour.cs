using Pathfinding;
using UnityEngine;

public class PatrolBehaviour : AIBehaviour
{
	[SerializeField] private AIPath ai;
	
	[Space]
	[SerializeField] private Transform[] patrolPoints;
	[SerializeField] private float delay = 0f;

	private int pointIndex = 0;
	private float nextPointTime = float.PositiveInfinity;
	
	private void Update()
	{
		if (patrolPoints.Length == 0 || !Enabled)
		{
			return;
		}

		bool search = false;
		
		if (ai.reachedEndOfPath && !ai.pathPending && float.IsPositiveInfinity(nextPointTime))
		{
			nextPointTime = Time.time + delay;
		}

		if (Time.time >= nextPointTime)
		{
			pointIndex++;
			search = true;
			if (pointIndex >= patrolPoints.Length)
			{
				pointIndex = 0;
			}
	
			nextPointTime = float.PositiveInfinity;
		}

		SetDestination(search);
	}
	
	private void SetDestination(bool search)
	{
		ai.destination = patrolPoints[pointIndex].position;

		if (search)
		{
			ai.SearchPath();
		}
	}

	protected override void OnBehaviourEnabled()
	{
		SetDestination(true);
	}
	
	protected override void OnBehaviourDisabled()
	{
		ai.SetPath(null);
		ai.destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
	}
}
