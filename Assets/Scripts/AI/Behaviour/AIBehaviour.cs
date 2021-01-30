using Pathfinding;
using UnityEngine;

public abstract class AIBehaviour : MonoBehaviour
{
	private bool behaviourEnabled = true;
	public bool Enabled
	{
		get => behaviourEnabled;
		set
		{
			if (behaviourEnabled != value)
			{
				behaviourEnabled = value;
				
				if (behaviourEnabled)
				{
					OnBehaviourEnabled();
				}
				else
				{
					OnBehaviourDisabled();
				}
			}
		}
	}
	
	public abstract AIPath AI { get; }

	protected abstract void OnBehaviourEnabled();
	protected abstract void OnBehaviourDisabled();

	public float GetSpeedPercent()
	{
		float speedPercent = AI.velocity.magnitude / AI.maxSpeed;
		return speedPercent;
	}
}
