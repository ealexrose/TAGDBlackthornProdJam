using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int waveSize;
    public float timeBetweenSpawns;
    bool waveActive;
    public float startPower;
    float power;
    List<GameObject> waveSpawners = new List<GameObject>();
    List<int> activeWaveSpawners;
    // Start is called before the first frame update
    void Start()
    {
        power = startPower;
        PrepNextWave();
        NextWave();
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
                GameManager.instance.WaveEnded();
            }

        }
    }

    public void AddSpawner(GameObject waveSpawner)
    {
        waveSpawners.Add(waveSpawner);
    }

    public void NextWave()
    {
        //Calcualte how much power should be availible for the next wave
        SpawnWave(power);
        power *= 1.35f;
    }

    public void PrepNextWave()
    {
        activeWaveSpawners = GetActiveSpawnersForWave();
        GiveActiveSpawnersPower(activeWaveSpawners, power);
        ShowDanger(activeWaveSpawners);
    }

    private void ShowDanger(List<int> activeWaveSpawners)
    {
        for (int i = 0; i < activeWaveSpawners.Count; i++)
        {
            waveSpawners[activeWaveSpawners[i]].GetComponent<WaveSpawner>().ShowDangerSymbol();
        }
    }

    private void HideDanger(List<int> activeWaveSpawners)
    {
        for (int i = 0; i < activeWaveSpawners.Count; i++)
        {
            waveSpawners[activeWaveSpawners[i]].GetComponent<WaveSpawner>().HideDangerSymbols();
        }
    }

    public void SpawnWave(float power)
    {
        waveActive = true;
        GameManager.BetweenWaves = false;
        HideDanger(activeWaveSpawners);
        ActivateSpawners(activeWaveSpawners);
    }

    public List<int> GetActiveSpawnersForWave() 
    {
        Debug.Log("Getting Active Spawners");
        List<int> roundSpawners = new List<int>();
        while (roundSpawners.Count <2)
        {
            for (int i = 0; i < waveSpawners.Count; i++) 
            {
                float randomRoll = Random.Range(0f, 1f);
                if (randomRoll < waveSpawners[i].GetComponent<WaveSpawner>().activtyChance) 
                {
                    roundSpawners.Add(i);
                }            
            }
        }

        for (int i = 0; i < waveSpawners.Count; i++)
        {
            WaveSpawner waveSpawner = waveSpawners[i].GetComponent<WaveSpawner>();
            waveSpawner.activtyChance = Mathf.Min(0.8f, waveSpawner.activtyChance + waveSpawner.activtyChanceGrowth);
        }
        Debug.Log(roundSpawners.Count);
        return roundSpawners;
    }

    public void GiveActiveSpawnersPower( List<int> activeSpawners, float totalPower) 
    {
        float totalPowerWeight = 0;
        for (int i = 0; i < activeSpawners.Count; i++) 
        {
            totalPowerWeight += waveSpawners[activeSpawners[i]].GetComponent<WaveSpawner>().weight;
        }
        for (int i = 0; i < activeSpawners.Count; i++)
        {
            WaveSpawner waveSpawner = waveSpawners[activeSpawners[i]].GetComponent<WaveSpawner>();
            waveSpawner.effectivePower = (waveSpawner.weight/totalPowerWeight) * totalPower;
        }
    }

    public void ActivateSpawners(List<int> activeSpawners) 
    {
        for (int i = 0; i < activeSpawners.Count; i++)
        {
            waveSpawners[activeSpawners[i]].GetComponent<WaveSpawner>().WaveLogic();
            waveSpawners[activeSpawners[i]].GetComponent<WaveSpawner>().StartWave();
        }
    }
}
