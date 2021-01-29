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
    bool Dead;
    #region User Interface Variables
    [SerializeField] Animator EndScreenUI;
    [SerializeField] Animator NextWaveNotification;
    [SerializeField] Animator NextWaveButton;
    [SerializeField] Animator HelpfulTooltip;           // useful incase the player does not know that they need to destroy towers.
    int Number_OF_Failures = 0;
    private TMPro.TextMeshProUGUI NextWaveTextBox;
    int WaveNotificationID;     // Hash ID of the Wave Animation

    string NextWaveIntermissionText = "Wave X Finished\nDestroy 1-3 towers to\npower up remaining towers";
    string NextWaveStartingText = "Next Wave Starting...";

    #endregion

    void Awake()
    {
        instance = this;
        WaveNotificationID = Animator.StringToHash("WaveNotification");
        NextWaveTextBox = NextWaveNotification.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        Wave_Manager = GetComponent<WaveManager>();
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.instance.GameIsPaused | Dead) { return; }        // bugs arise when we try to pause the game and then speed up. It changes the time scale when it is paused.
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
            SFX_Manager.Instance.PlayRandomButtonClick();
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
        if (Dead) { return; }
        NextWaveTextBox.fontSize = 10;
        NextWaveTextBox.text = NextWaveIntermissionText;            // replace x with the wave number
        NextWaveNotification.Play(WaveNotificationID, -1, 0);
        NextWaveButton.SetBool("WaveIntermission",true);
        BetweenWaves = true;
        Debug.Log("wave ended");
        
    }

    public void TryStartNextWave() 
    {
        if (TowerDestructionWithinMargin()) 
        {
            Number_OF_Failures = 0;
            HandleNextWaveUI();
            DestroyTowers();
            Wave_Manager.NextWave();
        }
        else
        {
            Number_OF_Failures++;
            if(Number_OF_Failures >= 3)
            {
                Number_OF_Failures = 0;
                HelpfulTooltip.Play(WaveNotificationID, -1, 0);
            }
        }
    }

    private void HandleNextWaveUI()
    {
        NextWaveNotification.Play(WaveNotificationID, -1, 0);
        NextWaveTextBox.fontSize = 15;
        NextWaveTextBox.text = NextWaveStartingText; // replace the "X" with the new wave number.
        NextWaveButton.SetBool("WaveIntermission", false);
    }

    private void DestroyTowers()
    {
        foreach (GameObject tower in towers) 
        {
            SFX_Manager.Instance.PlayRandomDeactivateTower();
            TowerController towerController = tower.GetComponent<TowerController>();
            if (towerController.towerSlatedForDestruction)
            {
                towerController.destroyed = true;
                tower.GetComponentInChildren<Animator>().SetTrigger("Deactivate");
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
        Dead = true;
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
