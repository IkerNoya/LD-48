using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLevelsManager : MonoBehaviour
{

    [SerializeField] private GameObject[] objectsLevels;
    int currentLevel = 0;

    void Start()
    {
        if (objectsLevels.Length > 0)
            objectsLevels[0].SetActive(true);
    }

    void OnEnable()
    {
        LevelManager.OnEnableGeneratedEnemys += OnNextLevel;
    }

    void OnDisable()
    {
        LevelManager.OnEnableGeneratedEnemys -= OnNextLevel;
    }

    void OnNextLevel(LevelManager lm)
    {
        if (lm.GetEnablePassLevel())
        {
            currentLevel++;

            for (int i = 0; i < objectsLevels.Length; i++)
            {
                objectsLevels[i].SetActive(false);
            }

            if (currentLevel < objectsLevels.Length && currentLevel >= 0)
                objectsLevels[currentLevel].SetActive(true);
        }
    }
}
