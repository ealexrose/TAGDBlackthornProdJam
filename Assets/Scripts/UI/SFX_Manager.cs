using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Manager : MonoBehaviour
{
    #region Static Instance
    private static SFX_Manager instance;
    public static SFX_Manager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SFX_Manager>();
                if (instance == null)
                {
                    instance = new GameObject("Spawned UI Button Manager", typeof(SFX_Manager)).GetComponent<SFX_Manager>();
                }
            }

            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    #endregion

    public AudioClip[] Button_Click_SFX;
    public AudioClip[] Base_Damage;
    public AudioClip[] Squeaks;
    public AudioClip[] Fire;
    public AudioClip[] DeactivateTower;
    // rat horde and rat horde big are not in here.

    private void Awake()
    {
        //Makes sure the instance doesn't get destroyed

        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayRandomButtonClick()
    {
        var sound = Button_Click_SFX[Random.Range(0, Button_Click_SFX.Length)];
        AudioManager.Instance.PlaySFX(sound);
    }

    public void PlayRandomBaseDamage()
    {
        var sound = Base_Damage[Random.Range(0, Base_Damage.Length)];
        AudioManager.Instance.PlaySFX(sound);
    }
    public void PlayRandomSqueaks()
    {
        var sound = Squeaks[Random.Range(0, Squeaks.Length)];
        AudioManager.Instance.PlaySFX(sound);
    }
    public void PlayRandomFire()
    {
        var sound = Fire[Random.Range(0, Fire.Length)];
        AudioManager.Instance.PlaySFX(sound);
    }
    public void PlayRandomDeactivateTower()
    {
        var sound = DeactivateTower[Random.Range(0, DeactivateTower.Length)];
        AudioManager.Instance.PlaySFX(sound);
    }
    public AudioClip GetRandomFire()
    {
        var sound = Fire[Random.Range(0, Fire.Length)];
        return sound;
    }
}
