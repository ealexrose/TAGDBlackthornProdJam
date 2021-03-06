﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool m_GameIsPaused = false;
    public bool GameIsPaused        // syncing property to UI
    {
        get
        {
            return m_GameIsPaused;
        }
        set
        {
            if(m_GameIsPaused != value)
            {
                m_GameIsPaused = value;
                if (m_GameIsPaused)
                {
                    Time.timeScale = 0f;
                    PauseMenu_UI.SetBool("Paused", true);
                }
                else
                {
                    Time.timeScale = 1f;
                    PauseMenu_UI.SetBool("Paused", false);
                }
            }
        }

    }

    private Animator PauseMenu_UI;

    public static PauseMenu instance;

    private void Awake()
    {
        PauseMenu_UI = GetComponent<Animator>();
    }

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameIsPaused = !GameIsPaused;
        }
    }

    public void ChangePausedState(bool State)
    {
        SFX_Manager.Instance.PlayRandomButtonClick();
        GameIsPaused = State;
    }

    public void QuitToMenu()
    {
        // quit to menu or quit game?
        SFX_Manager.Instance.PlayRandomButtonClick();
        SceneManager.LoadScene(0);
    }
    
}
