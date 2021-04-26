using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOptions : MonoBehaviour
{
    [SerializeField] public Slider sfxVolume;
    [SerializeField] public Slider musicVolume;

    [SerializeField] public Slider verticalPlayerSensi;
    [SerializeField] public Slider horizontalPlayerSensi;

    [SerializeField] public Toggle playerToggleCrouch;
    [SerializeField] public Toggle playerToggleSprint;

    [SerializeField] public GameObject panelAudio;
    [SerializeField] public GameObject panelControl;

    private float sfxVolu = 0;
    private float musicVolu = 0;
    private float verticalValue = 0;
    private float horizontalValue = 0;
    private bool togPjCrouch = false;
    private bool togPjSprint = false;
    void Awake()
    {
        sfxVolume.value = DataManager.instance.GetSFXVolume();
        musicVolume.value = DataManager.instance.GetMusicVolume();
        verticalPlayerSensi.value = DataManager.instance.GetVerticalSesitivity();
        horizontalPlayerSensi.value = DataManager.instance.GetHorizontalSesitivity();

        playerToggleCrouch.isOn = DataManager.instance.GetPlayerToggleCrouch();
        playerToggleSprint.isOn = DataManager.instance.GetPlayerToggleSprint();

    }
    void Update()
    {
        sfxVolu = sfxVolume.value;        
        musicVolu = musicVolume.value;
        togPjCrouch = playerToggleCrouch.isOn;
        togPjSprint = playerToggleSprint.isOn;
        verticalValue = verticalPlayerSensi.value;
        horizontalValue = horizontalPlayerSensi.value;

        DataManager.instance.SetSFXVolume(sfxVolu);
        DataManager.instance.SetMusicVolume(musicVolu);

        DataManager.instance.SetTogglePlayerCrouch(togPjCrouch);
        DataManager.instance.SetTogglePlayerSprint(togPjSprint);
        DataManager.instance.SetVerticalSensitivity(verticalValue);
        DataManager.instance.SetHorizontalSensitivity(horizontalValue);
    }
    public void ActivePanelAudio()
    {
        panelAudio.SetActive(true);
        panelControl.SetActive(false);
    }
    public void ActivePanelControls()
    {
        panelAudio.SetActive(false);
        panelControl.SetActive(true);
    }
}
