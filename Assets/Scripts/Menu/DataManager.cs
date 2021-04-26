using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [SerializeField]private float musicVolume;
    [SerializeField]private float sfxVolume;

    [SerializeField] private bool playerCrouch;
    [SerializeField] private bool playerSprint;

    [SerializeField] private float verticalSensitivity;
    [SerializeField] private float horizontalSensitivity;
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
        AudioManager.instance.SetVolumeSFX();
    }
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        AudioManager.instance.SetVolumeMusic();
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

    #region PLAYER OPTIONS
    public void SetTogglePlayerCrouch(bool value)
    {
        playerCrouch = value;
    }
    public void SetTogglePlayerSprint(bool value)
    {
        playerSprint = value;
    }
    public bool GetPlayerToggleCrouch()
    {
        return playerCrouch;
    }
    public bool GetPlayerToggleSprint()
    {
        return playerSprint;
    }
    public void SetVerticalSensitivity(float value)
    {
        verticalSensitivity = value;
    }
    public void SetHorizontalSensitivity(float value)
    {
        horizontalSensitivity = value;
    }
    public float GetHorizontalSesitivity()
    {
        return horizontalSensitivity;
    }
    public float GetVerticalSesitivity()
    {
        return verticalSensitivity;
    }
    #endregion
}
