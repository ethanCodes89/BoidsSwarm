using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmSpawner : MonoBehaviour
{
    [SerializeField] private GameObject swarmPrefab;
    [SerializeField] private int numberToSpawn = 10;
    [SerializeField] private float spawnRadius = 10f;

    private void Start()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
            randomPosition += transform.position;

            Instantiate(swarmPrefab, randomPosition, Quaternion.identity);
        }
    }
}
