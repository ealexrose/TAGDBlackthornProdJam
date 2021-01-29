using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndScreen : MonoBehaviour
{
    public void Retry()
    {
        SFX_Manager.Instance.PlayRandomButtonClick();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu()
    {
        SFX_Manager.Instance.PlayRandomButtonClick();
        SceneManager.LoadScene(0);
    }
}
