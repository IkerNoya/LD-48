using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMnager : MonoBehaviour
{
    private static GameMnager instance;
    public static GameMnager Get()
    {
        return instance;
    }
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
    void Update()
    {
        
    }

    public void CheckIfPause()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }
}
