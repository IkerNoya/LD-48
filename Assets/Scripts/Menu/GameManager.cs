using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Get()
    {
        return instance;
    }

    private bool isPaused;
    private float auxTimeScale;
    
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        auxTimeScale = Time.timeScale;
        DontDestroyOnLoad(gameObject);
    }

    public bool IsPauseOn()
    {
        if (isPaused)return true;
        else return false;
    }

    public void SetPause()
    {
        isPaused = !isPaused;
        if (isPaused)
            Time.timeScale = 0;
        else
            Time.timeScale = auxTimeScale;

        Debug.Log("Time Scale: " + Time.timeScale.ToString());
    }
}
