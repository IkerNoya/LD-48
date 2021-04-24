using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOptions : MonoBehaviour
{
    [SerializeField] public Slider sfxVolume;
    [SerializeField] public Slider musicVolume;

    private float sfxVolu = 0;
    private float musicVolu = 0;
    void Awake()
    {
        sfxVolume.value = DataManager.instance.GetSFXVolume();
        musicVolume.value = DataManager.instance.GetMusicVolume();
    }
    void Update()
    {
        sfxVolu = sfxVolume.value;        
        musicVolu = musicVolume.value;
        DataManager.instance.SetSFXVolume(sfxVolu);
        DataManager.instance.SetMusicVolume(musicVolu);
        //Audio manager seteo de volumen
    }
}
