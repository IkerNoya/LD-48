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

    private bool isPaused = false;
    private float auxTimeScale;
    
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

    private void Update()
    {
        if(Time.timeScale != 0)
            auxTimeScale = Time.timeScale;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPause();
        }
    }

    public void SetPause()
    {
        isPaused = !isPaused;
        Debug.Log("Is pause: " + isPaused.ToString());
        if (isPaused)
            Time.timeScale = 0;
        else
            Time.timeScale = auxTimeScale;

        Debug.Log("Time.timeScale: " + Time.timeScale);
    }
}
