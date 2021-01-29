using UnityEngine;
using Pathfinding;

public class DestinationSetter : MonoBehaviour
{
	[Space]
	[SerializeField] private AIPath ai;
	[SerializeField] private Transform target;
	
	void Update() 
	{
		if (target && ai)
		{
			ai.destination = target.position;
		}
	}

	public void Cancel()
	{
		ai.SetPath(null);
		ai.destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
	}
}
