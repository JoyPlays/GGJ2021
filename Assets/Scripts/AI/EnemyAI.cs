using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	[SerializeField] private AIBehaviour aiBehaviour;
	[SerializeField] private AgroBehaviour agroBehaviour;

	[SerializeField] private float agroRadius = 5f;

	[SerializeField] private LayerMask agroLayerMask;

	private void Start()
	{
		agroBehaviour.Enabled = false;
	}

	private void Update()
	{
		if (!agroBehaviour.Enabled)
		{
			Collider[] agroResult = new Collider[10];
			Physics.OverlapSphereNonAlloc(transform.position, agroRadius, agroResult, agroLayerMask);

			foreach (var agroTarget in agroResult)
			{
				if (agroTarget)
				{
					aiBehaviour.Enabled = false;
					agroBehaviour.SetupAgroTarget(agroTarget.transform);
					agroBehaviour.Enabled = true;
					break;
				}
			}
		}
	}

	public void AgroOutOfRange()
	{
		aiBehaviour.Enabled = true;
	}

	private void OnDrawGizmos()
	{
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		Gizmos.color = new Color(0.75f, 0, 0, 0.7f);
		Gizmos.DrawSphere(Vector3.zero, agroRadius);
	}
}

