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
    WaveManager Wave_Manager;

    // Start is called before the first frame update 
    public static GameManager instance;
    bool fastforwarding;

    #region User Interface Variables
    [SerializeField] Animator EndScreenUI;
    [SerializeField] Animator NextWaveNotification;
    [SerializeField] Animator NextWaveButton;
    int WaveNotificationID;     // Hash ID of the Wave Animation

    string NextWaveText = "Wave X Finished \nDestroy towers to power up remaining towers";
    string NextWaveStartingText = "Next Wave Starting...";

    #endregion

    void Awake()
    {
        instance = this;
        WaveNotificationID = Animator.StringToHash("WaveNotification");
        Wave_Manager = GetComponent<WaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.instance.GameIsPaused) { return; }        // bugs arise when we try to pause the game and then speed up. It changes the time scale when it is paused.
        if (Input.GetButtonDown("Jump") && BetweenWaves) 
        {
            TryStartNextWave();
        }
        if (Input.GetButtonDown("Horizontal"))
        {
            FastForward();
        }
        else if (Input.GetButtonUp("Horizontal")) 
        {
            NormalSpeed();
        }
    }

    public void FastForward()
    {
        if (fastforwarding) 
        {
            return;
        }
        else
        {
            Time.timeScale = 5f;
            fastforwarding = true;
            return;
        }
    }

    public void NormalSpeed()
    {
        if (!fastforwarding)
        {
            return;
        }
        else
        {
            Time.timeScale = 1f;
            fastforwarding = false;
            return;
        }
    }

    public void WaveEnded() 
    {
        NextWaveNotification.Play(WaveNotificationID);
        NextWaveButton.SetBool("WaveIntermission",true);
        BetweenWaves = true;
        Debug.Log("wave ended");
    }

    void TryStartNextWave() 
    {
        if (TowerDestructionWithinMargin()) 
        {
            DestroyTowers();
            Wave_Manager.NextWave();
        }
    }

    private void DestroyTowers()
    {
        foreach (GameObject tower in towers) 
        {
            TowerController towerController = tower.GetComponent<TowerController>();
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

    public void LoseGame() 
    {
        EndScreenUI.SetTrigger("GameEnded");
        // do other stuff here
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
