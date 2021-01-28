using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemy;
    public Transform Base;

    //How often this spawner will get used
    public float activtyChance;
    public float activtyChanceGrowth;


    //How much power to distribute
    public float weight;
    public float weightGrowth;
    [HideInInspector]
    public float effectivePower;

    //How often this spawner uses specific wave types
    public float[] spawnModeWeights;
    public float[] spawnModeGrowth;

    [HideInInspector]
    public bool spawning;
    List<Wave> waveParts;

    // Start is called before the first frame update
    void Awake()
    {
        FindObjectOfType<WaveManager>().AddSpawner(this.gameObject);
    }

    public void StartWave()
    {
        spawning = true;
        StartCoroutine(EnemySpawn());
    }

    public void WaveLogic()
    {
        waveParts = new List<Wave>();
        float totalPowerToSpend = effectivePower;
        while (effectivePower > 0)
        {
            Debug.Log("crafting wave");
            int wavePartType = PickWaveType();
            float wavePartPower = UnityEngine.Random.Range(totalPowerToSpend / 4f, effectivePower);
            effectivePower -= wavePartPower;
            float wavePartSpacer = UnityEngine.Random.Range(2f, 6f);
            Wave wavepart = new Wave(wavePartType, wavePartPower, wavePartSpacer);
            waveParts.Add(wavepart);

        }

    }

    private int PickWaveType()
    {
        float totalWeight = 0;

        foreach (float weight in spawnModeWeights)
        {
            totalWeight += weight;
        }

        float selectedWeight = UnityEngine.Random.Range(0, totalWeight);
        float countedWeight = 0;
        for (int i = 0; i < spawnModeWeights.Length; i++)
        {
            countedWeight += spawnModeWeights[i];
            if (selectedWeight < countedWeight)
            {
                return i;
            }
        }

        Debug.LogError("Wave Part Type not selected");
        return 0;
    }

    public IEnumerator EnemySpawn()
    {
        foreach (Wave wavePart in waveParts)
        {
            yield return new WaitForSeconds(wavePart.startDelayTime);
            switch (wavePart.spawnMode)
            {
                case Wave.SpawnMode.LongStream:
                    float powerForSpawnInterval = UnityEngine.Random.Range(wavePart.dedicatedPower / 25f, wavePart.dedicatedPower / 10f);
                    float powerForSpawnCount = wavePart.dedicatedPower - powerForSpawnInterval;
                    float timeBetweenSpawns = LongStreamPowerToTime(powerForSpawnInterval);
                    int remainingWaveEnemies = LongStreamPowerToEnemies(powerForSpawnCount, powerForSpawnInterval);
                    Debug.Log(remainingWaveEnemies);
                    for (int i = 0; i < remainingWaveEnemies; i++)
                    {
                        yield return new WaitForSeconds(timeBetweenSpawns);
                        GameObject clone = Instantiate(enemy, transform.position, Quaternion.identity, null);
                        clone.GetComponent<AIDestinationSetter>().target = Base;
                        Debug.Log(clone.GetComponent<AIDestinationSetter>().target);
                        yield return new WaitForSeconds(timeBetweenSpawns);
                    }
                    break;
                case Wave.SpawnMode.StutteredBurst:
                    break;
                case Wave.SpawnMode.BossSolo:
                    break;
                case Wave.SpawnMode.BossStream:
                    break;
            }
        }
        spawning = false;

    }

    private int LongStreamPowerToEnemies(float powerForSpawnCount, float powerForSpawnInterval)
    {
        int enemiesToSpawn = 0;
        enemiesToSpawn = (int)(powerForSpawnCount * ((Mathf.Sqrt(powerForSpawnInterval) / 0.56f) * 0.1f));
        return enemiesToSpawn;
    }

    private float LongStreamPowerToTime(float spentPower)
    {
        float convertedTime = 0;
        convertedTime = .56f / (Mathf.Pow(spentPower, 9f / 8f) / 2f);
        return convertedTime;
    }
}
