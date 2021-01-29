using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	[SerializeField] private AIBehaviour aiBehaviour;

	[SerializeField] private float agroRadius = 5f;

	private void Update()
	{
		Collider[] agroResult = new Collider[10];
		Physics.OverlapSphereNonAlloc(transform.position, agroRadius, agroResult, 2);
	}
}

