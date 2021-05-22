using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    private float spawnRate = 7.0f; //the initial spawn interval
    private int intervalRate = 100;  //the interval spawn will increas 1% with the level up

    private GameManager gameMangerScript;
    private bool isSpawnStart = false;
    // Start is called before the first frame update
    void Start()
    {
        gameMangerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private bool SpawnAvaliableCheck()
    {
        return gameMangerScript.GameInProgress();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSpawnStart && SpawnAvaliableCheck())
        {
            SpawnAnObstacle();
            isSpawnStart = true;
        }
    }

    // Spawn an obstacle
    private void SpawnAnObstacle()
    {
        if(SpawnAvaliableCheck())
        {
            int randomIndex = Random.Range(0, obstaclePrefabs.Length);
            Vector3 spawnPos = new Vector3(transform.position.x, obstaclePrefabs[randomIndex].transform.position.y, transform.position.z);
            Instantiate(obstaclePrefabs[randomIndex], transform.position, obstaclePrefabs[randomIndex].transform.rotation);
            spawnRate *= (1 - gameMangerScript.gameLevel / intervalRate);
            Invoke("SpawnAnObstacle", spawnRate);
        }

    }
}
