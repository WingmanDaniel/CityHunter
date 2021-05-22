using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    private float spawnRate = 7.0f;         //the initial spawn interval
    private float intervalRate = 50.0f;     //the interval spawn will increas 2% with the level up and must be less than the total level of Game Manager
    private GameManager gameMangerScript;
    private bool isSpawnStart = false;

    // Start is called before the first frame update
    // 1. Assigning values to variables
    void Start()
    {
        gameMangerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    // 1. call a spawn method when the spawn situation meets the requirements
    void Update()
    {
        if(!isSpawnStart && SpawnAvaliableCheck())
        {
            SpawnAnObstacle();
            isSpawnStart = true;
        }
    }

    //check whether the spawn situation meets the requirements
    private bool SpawnAvaliableCheck()
    {
        return gameMangerScript.GameInProgress();
    }

    // Spawn an obstacle
    //1. random choose a obstacle prefab
    //2. based on the current level to excute the spawn gap
    private void SpawnAnObstacle()
    {
        if(SpawnAvaliableCheck())
        {
            int randomIndex = Random.Range(0, obstaclePrefabs.Length);
            Vector3 spawnPos = new Vector3(transform.position.x, obstaclePrefabs[randomIndex].transform.position.y, transform.position.z);
            Instantiate(obstaclePrefabs[randomIndex], transform.position, obstaclePrefabs[randomIndex].transform.rotation);
            float currentSpawnRate = spawnRate * (1 - (gameMangerScript.gameLevel - 1) / intervalRate);
            Debug.Log(currentSpawnRate);
            Invoke("SpawnAnObstacle", currentSpawnRate);
        }

    }
}
