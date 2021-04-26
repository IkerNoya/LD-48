using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] public Canvas pauseMenu;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
            GameManager.Get().SetPause();
        }
    }
}
