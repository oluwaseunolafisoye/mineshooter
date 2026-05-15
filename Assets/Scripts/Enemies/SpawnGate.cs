using System.Collections;
using UnityEngine;

public class SpawnGate : MonoBehaviour
{
    [SerializeField] GameObject robotPrefab;
    [SerializeField] float spawnTime = 5f;
    [SerializeField] Transform spawnPoint;
    [SerializeField] int maxRobots = 5;

    PlayerHealth player;
    int spawned;

    void Start()
    {
        player = FindFirstObjectByType<PlayerHealth>();
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (player && spawned < maxRobots)
        {
            Instantiate(robotPrefab, spawnPoint.position, transform.rotation);
            spawned++;
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
