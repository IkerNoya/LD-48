using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader instance;
    public static SceneLoader Get()
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
    public void Start()
    {
            AudioManager.instance.Play("MENU");
    }
    public void LoadScene(string name)
    {
        if (name == "MainLevel" || name == "Faka 2")
        {
            AudioManager.instance.Stop("MENU");
            AudioManager.instance.Play("START");
            AudioManager.instance.Play("GAME");
        }
        else
            AudioManager.instance.Play("OPTION");

        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
    public Scene GetActualScene()
    {
        return SceneManager.GetActiveScene();
    }
    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
