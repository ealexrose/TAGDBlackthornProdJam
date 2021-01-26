using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemy;
    public Transform Base;
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
            GameObject clone = Instantiate(enemy, transform.position, Quaternion.identity, null);
            remainingWaveEnemies--;
            clone.GetComponent<AIDestinationSetter>().target = Base;
            Debug.Log(clone.GetComponent<AIDestinationSetter>().target);
            StartCoroutine(EnemySpawn(remainingWaveEnemies, timeBetweenSpawns));
        }
        else 
        {
            spawning = false;
        }

    }
}
