using System.Collections;
using System.Collections.Generic;
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

	protected abstract void OnBehaviourEnabled();
	protected abstract void OnBehaviourDisabled();
}
