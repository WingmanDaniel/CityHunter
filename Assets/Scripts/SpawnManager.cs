using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    private float spawnRate = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnAnObstacle", spawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawn an obstacle
    private void SpawnAnObstacle()
    {
        int randomIndex = Random.Range(0, obstaclePrefabs.Length);
        Vector3 spawnPos = new Vector3(transform.position.x, obstaclePrefabs[randomIndex].transform.position.y, transform.position.z);
        Instantiate(obstaclePrefabs[randomIndex], transform.position, obstaclePrefabs[randomIndex].transform.rotation);
        Invoke("SpawnAnObstacle", spawnRate);
    }
}
