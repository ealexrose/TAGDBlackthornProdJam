using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int waveSize;
    public float timeBetweenSpawns;
    bool waveActive;
    List<GameObject> waveSpawners = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnWave(waveSize, timeBetweenSpawns);
    }

    // Update is called once per frame
    void Update()
    {
        if (waveActive) 
        {
            bool spawnersActive = false;
            foreach (GameObject waveSpawner in waveSpawners)
            {

                if (waveSpawner.GetComponent<WaveSpawner>().spawning) 
                {
                    spawnersActive = true;
                }
            }

            if (!spawnersActive && !FindObjectOfType<EnemyController>())
            {
                waveActive = false;
                GameManager.WaveEnded();
            }

        }
    }

    public void AddSpawner(GameObject waveSpawner)
    {
        waveSpawners.Add(waveSpawner);
    }

    public void SpawnWave(int enemyCount, float timeBetweenSpawns)
    {
        waveActive = true;
        GameManager.BetweenWaves = false;

        int laneSplit = enemyCount / waveSpawners.Count;
        foreach (GameObject waveSpawner in waveSpawners)
        {
            waveSpawner.GetComponent<WaveSpawner>().StartWave(laneSplit, timeBetweenSpawns);
        }
    }

}
