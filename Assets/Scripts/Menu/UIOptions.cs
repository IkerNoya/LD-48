using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOptions : MonoBehaviour
{
    [SerializeField] public Slider sfxVolume;
    [SerializeField] public Slider musicVolume;

    [SerializeField] public Toggle playerToggleCrouch;
    [SerializeField] public Toggle playerToggleSprint;

    [SerializeField] public GameObject panelAudio;
    [SerializeField] public GameObject panelControl;

    private float sfxVolu = 0;
    private float musicVolu = 0;
    private bool togPjCrouch = false;
    private bool togPjSprint = false;
    void Awake()
    {
        sfxVolume.value = DataManager.instance.GetSFXVolume();
        musicVolume.value = DataManager.instance.GetMusicVolume();
        playerToggleCrouch.isOn = DataManager.instance.GetPlayerToggleCrouch();
        playerToggleSprint.isOn = DataManager.instance.GetPlayerToggleSprint();
    }
    void Update()
    {
        sfxVolu = sfxVolume.value;        
        musicVolu = musicVolume.value;
        togPjCrouch = playerToggleCrouch.isOn;
        togPjSprint = playerToggleSprint.isOn;
        DataManager.instance.SetSFXVolume(sfxVolu);
        DataManager.instance.SetMusicVolume(musicVolu);
        DataManager.instance.SetTogglePlayerCrouch(togPjCrouch);
        DataManager.instance.SetTogglePlayerSprint(togPjSprint);
        //Audio manager seteo de volumen
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
