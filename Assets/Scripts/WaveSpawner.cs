using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemy;
    [HideInInspector]
    public bool spawning;

    // Start is called before the first frame update
    void Awake()
    {
        FindObjectOfType<WaveManager>().AddSpawner(this.gameObject);
    }

    public void StartWave(int enemyCount, float timeBetweenSpawns)
    {
        spawning = true;
        StartCoroutine(EnemySpawn(enemyCount, timeBetweenSpawns));
    }


    public IEnumerator EnemySpawn(int remainingWaveEnemies, float timeBetweenSpawns)
    {

        if (remainingWaveEnemies > 0)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            Instantiate(enemy, transform.position, Quaternion.identity, null);
            remainingWaveEnemies--;
            StartCoroutine(EnemySpawn(remainingWaveEnemies, timeBetweenSpawns));
        }
        else 
        {
            spawning = false;
        }

    }
}
