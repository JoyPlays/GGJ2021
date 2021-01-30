using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFx : MonoBehaviour
{
	[SerializeField] private Light fireLight;
	[SerializeField] private float maxValue;
	[SerializeField] private float minValue;

	private void Start()
	{
		StartCoroutine("FireLightAdjust");
	}

	IEnumerator FireLightAdjust()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.05f);
			float t = 0f;
			while (t < 5f)
			{
				if (fireLight.intensity >= maxValue)
				{
					fireLight.intensity = Mathf.Lerp(maxValue, minValue, Time.deltaTime);
				}
				if (fireLight.intensity <= minValue)
				{
					fireLight.intensity = Mathf.Lerp(minValue, maxValue, Time.deltaTime);
				}
				t += Time.deltaTime;
			}
			
		}
		
	}
}
