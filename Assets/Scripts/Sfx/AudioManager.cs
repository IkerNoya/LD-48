using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = DataManager.instance.GetSFXVolume();
            s.source.pitch = Time.timeScale;
            s.source.loop = s.loop;
        }
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void Play(string name)
    {
       Sound s= Array.Find(sounds, sound => sound.name == name);

        if (s != null && !s.source.isPlaying)
            s.source.Play();
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null && s.source.isPlaying)
            s.source.Stop();
    }
    public void SetVolumeSFX()
    {
        foreach (Sound s in sounds)
        {
            if(s.type == Sound.Type.sound)
                s.source.volume = DataManager.instance.GetSFXVolume();
        }
    }
    public void SetVolumeMusic()
    {
        foreach (Sound s in sounds)
        {
            if(s.type == Sound.Type.music)
                s.source.volume = DataManager.instance.GetMusicVolume();
        }
    }
    void Update()
    {
        foreach (Sound s in sounds)
        {
            if(s.source.isPlaying)
                s.source.pitch = Time.timeScale;
        }
    }
}
