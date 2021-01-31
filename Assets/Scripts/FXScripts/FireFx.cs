using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFx : MonoBehaviour
{
	[SerializeField] private Light fireLight;
	[SerializeField] private float maxValue;
	[SerializeField] private float minValue;
	[SerializeField] private float iterationTime = 1f;

	private bool toMax = true;
	
	private void Start()
	{
		_ = StartCoroutine(FireLightAdjust());
	}

	private IEnumerator FireLightAdjust()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.05f);
			float t = 0f;
			while (t < 1f)
			{
				t += Time.deltaTime / iterationTime;
				
				if (toMax)
				{
					fireLight.intensity = Mathf.Lerp(minValue, maxValue, t);
				}
				else
				{
					fireLight.intensity = Mathf.Lerp(maxValue, minValue, t);
				}
				
				if (fireLight.intensity >= maxValue)
				{
					toMax = false;
				}
				else if (fireLight.intensity <= minValue)
				{
					toMax = true;
				}
				
				yield return null;
			}
			
		}
		
	}
}
