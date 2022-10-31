using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public List<Vector3> spawnLocations;
    public HUDControl hudControl;

    //Spawn system planning

    // n # enemy prefabs
    // spawn by wave or continuous? continuous is harder
    // minimum enemies (wave 1) 2
    // for( i = 0; i < minSpawn + waveNum; i++) spawn enemies at random spawn points
    // only spawn enemies below a difficulty threshhold
    // difficulty threshold increases at x points


    [Header("Spawn Modifiers")]
    
    public int minimumSpawnCount = 3; // Minimum enemies that spawn in a wave
    public int additionalSpawns = 0; // Extra spawn option
    private bool isscanning = false;
    private bool isAnyEnemyAlive = false; // Checks for remaining enemies before spawning next wave
    public bool isSpawning = false; //Is the spawn system currently on (spawning enemies)
    public float spawnDelayTime = 2.0f; //Time between enemy spawns
    public int waveNumber = 0;
    public int difficultyThreshold = 0; //Determines types of enemies
    public GameObject[] currentlyAliveEnemies;
    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        hudControl = GameObject.FindGameObjectWithTag("UI").GetComponent<HUDControl>();
        hudControl.UpdateWaveText(waveNumber);
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

   


    private void SpawnControl()
    {
        if(!isscanning)
        {
            StartCoroutine(EnemyScan());
        }
        if(!isAnyEnemyAlive && !isSpawning) //If no enemies are alive and spawning isnt active -> start spawning
        {
            waveNumber++;
            hudControl.UpdateWaveText(waveNumber);
            if (waveNumber % 3 == 0)
            {
                Debug.Log("wave mod 3 triggered");
                minimumSpawnCount += 1;
                difficultyThreshold += 1;
            }


            if (waveNumber % 10 == 0)
                additionalSpawns += 1;
            isAnyEnemyAlive = true;
            coroutine = SpawnEnum(spawnDelayTime);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator SpawnEnum(float delay)
    {
        isSpawning = true;
        Debug.Log("Is Spawning: " + isSpawning + " -  Time: " + Time.time);
        for (int i = 0; i < minimumSpawnCount + additionalSpawns; i++)
        {
            //Random Enemy
            int enemySelect = SelectEnemyToSpawn();

            //Random Spawn Location
            int spawnSelect = Random.Range(0, spawnLocations.Count);

            Debug.Log("Spawning " + enemyPrefabs[enemySelect].name + " at Spawn location " + spawnSelect + "Time: " + Time.time);
            Instantiate(enemyPrefabs[enemySelect], spawnLocations[spawnSelect], Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }
        isSpawning = false;
        Debug.Log("Is Spawning: " + isSpawning + " -  Time: " + Time.time);
    }

    private int SelectEnemyToSpawn()
    {
        int enemySelect;
        // Select an enemy equal or less than difficulty threshold
        do
        {
            enemySelect = Random.Range(0, enemyPrefabs.Count);
        } while (enemyPrefabs[enemySelect].GetComponent<EnemyInformation>().ReturnDifficultyLevel() > difficultyThreshold);
        //
        return enemySelect;

    }

    private IEnumerator EnemyScan()
    {
        isscanning = true;
        currentlyAliveEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (currentlyAliveEnemies.Length == 0)
        {
            isAnyEnemyAlive = false;
        }
        else
            isAnyEnemyAlive = true;
        yield return new WaitForSeconds(2);
        isscanning = false;
    }
}
