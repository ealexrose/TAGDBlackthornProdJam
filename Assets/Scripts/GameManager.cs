using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static bool BetweenWaves;
    public int minTowerDestruction;
    public int maxTowerDestruction;
    public int towersSlatedForDestruction;
    List<GameObject> towers = new List<GameObject>();


    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && BetweenWaves) 
        {
            TryStartNextWave();
        }
    }

    public static void WaveEnded() 
    {
        BetweenWaves = true;
        Debug.Log("wave ended");
    }

    void TryStartNextWave() 
    {
        if (TowerDestructionWithinMargin()) 
        {
            DestroyTowers();
            GetComponent<WaveManager>().SpawnWave(10,.2f);
        }
    }

    private void DestroyTowers()
    {
        foreach (GameObject tower in towers) 
        {
            TowerController towerController = tower.GetComponent<TowerController>();
            Debug.Log(tower);
            if (towerController.towerSlatedForDestruction)
            {
                towerController.destroyed = true;
            }
            else if(!towerController.destroyed)
            {
                towerController.damage += towersSlatedForDestruction;
            }
        }
        towersSlatedForDestruction = 0;
    }

    public static void LoseGame() 
    {
        throw new NotImplementedException("Game Loss not implemented");
    }
    public int AddTower(GameObject tower) 
    {
        towers.Add(tower);
        return towers.Count;
    }

    public bool TowerDestructionWithinMargin() 
    {
        if (towersSlatedForDestruction >= minTowerDestruction && towersSlatedForDestruction <= maxTowerDestruction) 
        {
            return true;
        }
        return false;
    
    }
}
