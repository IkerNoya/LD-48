using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] public Canvas pauseMenu;

    void Update()
    {
        if(Time.timeScale == 0)
           pauseMenu.gameObject.SetActive(true);
        else
           pauseMenu.gameObject.SetActive(false);
    }
}
