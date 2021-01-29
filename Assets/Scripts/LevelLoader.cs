using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetMouseButtonDown(0))
        {
            LoadNextLevel();
        }*/
    }

    public void LoadNextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        if(nextIndex >= SceneManager.sceneCountInBuildSettings){
            return;
        }

        StartCoroutine(LoadLevel(nextIndex));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
        
        SceneManager.LoadScene(levelIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}