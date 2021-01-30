using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPoints : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] escapePoints;

    [SerializeField] private CharacterController characterController;

    public static WorldPoints inst;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Gizmos.DrawSphere(spawnPoints[i].position, 1f);
        }

        /*
        Gizmos.color = Color.green;
        for (int i = 0; i < escapePoints.Length; i++)
        {
            Gizmos.DrawSphere(escapePoints[i].position, 1f);
        }
        */
    }

    private void Awake()
    {
        if (inst != null)
        {
            Destroy(this);
        }

        if (inst == null)
        {
            inst = this;
        }
    }

    private void Start()
    {
        SpawnPlayer();
    }

    private Vector3 GetSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].position;
    }

    public void SpawnPlayer()
    {
        characterController.transform.position = GetSpawnPoint();
    }
}
