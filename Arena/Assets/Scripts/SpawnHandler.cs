using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public List<Vector3> spawnLocations;

    // Start is called before the first frame update
    void Start()
    {
        // Create a list of all possible enemies - use to spawn them in randomly later
        /*
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Enemy").Length; i++)
        {
            enemyPrefabs.Add(GameObject.FindGameObjectsWithTag("Enemy")[i]);
        }
        */

        // Create a list of all possible enemy spawn Locations
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("EnemySpawn").Length; i++)
        {
            spawnLocations.Add(GameObject.FindGameObjectsWithTag("EnemySpawn")[i].transform.position);
        }
    }

    // Update is called once per frame


    void Update()
    {
        SpawnControl();
    }

    //Spawn system planning

    // n # enemy prefabs
    // spawn by wave or continuous? continuous is harder
    // minimum enemies (wave 1) 2
    // for( i = 0; i < minSpawn + waveNum; i++) spawn enemies at random spawn points
    // only spawn enemies below a difficulty threshhold
    // difficulty threshold increases at x points
    private int difficultyThreshold = 1; //Determines types of enemies
    private int minimumSpawnCount = 3; // Minimum enemies that spawn in a wave
    private int additionalSpawns = 0; // Extra spawn option
    private bool isAnyEnemyAlive = false; // Checks for remaining enemies before spawning next wave
    private float spawnDelayTime = 2.0f; //Time between enemy spawns
    private IEnumerator coroutine;
    private void SpawnControl()
    {
        if(!isAnyEnemyAlive) //Check for enemies present
        {
            isAnyEnemyAlive = true;
            coroutine = SpawnEnum(spawnDelayTime);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator SpawnEnum(float delay)
    {
        for (int i = 0; i < minimumSpawnCount + additionalSpawns; i++)
        {
            //Random Enemy
            int enemySelect = Random.Range(0, enemyPrefabs.Count);
            if (enemySelect > difficultyThreshold)
            {
                enemySelect = difficultyThreshold - 1;
            }
            //Random Spawn Location
            int spawnSelect = Random.Range(0, spawnLocations.Count);
            yield return new WaitForSeconds(2.0f);
            Debug.Log("Spawning " + enemyPrefabs[enemySelect].name + " at Spawn location " + spawnSelect);
            Instantiate(enemyPrefabs[enemySelect], spawnLocations[spawnSelect], Quaternion.identity);
        }
    }
}
