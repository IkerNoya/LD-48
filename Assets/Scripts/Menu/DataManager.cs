using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [SerializeField]private float musicVolume;
    [SerializeField]private float sfxVolume;
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region OPTIONS VALUES
    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
    }
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
    }
    public float GetMusicVolume()
    {
        return musicVolume;
    }
    public float GetSFXVolume()
    {
        return sfxVolume;
    }
    #endregion
}
