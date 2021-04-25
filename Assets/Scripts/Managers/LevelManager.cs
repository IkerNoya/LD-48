using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelManager : MonoBehaviour
{
    public static event Action<LevelManager> OnEnableGeneratedEnemys;

    public static LevelManager instance;

    [HideInInspector] public bool enableGeneratedEnemys;

    public int countEnemysDieForPassLevel;

    private int auxCountEnemysDieForPassLevel;

    [HideInInspector] public int needEnemysDieForNextGenerationsEnemys;

    private int currentEnemysDie;

    private bool enablePassLevel;

    void OnEnable()
    {
        Enemy.OnDieEnemy += OnEnemyDie;
    }

    void OnDisable()
    {
        Enemy.OnDieEnemy -= OnEnemyDie;
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    void Start()
    {
        enablePassLevel = false;
        auxCountEnemysDieForPassLevel = countEnemysDieForPassLevel;
    }

    private void OnEnemyDie(Enemy e)
    {
        currentEnemysDie++;
        countEnemysDieForPassLevel--;
        if (needEnemysDieForNextGenerationsEnemys > 0)
            needEnemysDieForNextGenerationsEnemys--;
        else
        {
            enableGeneratedEnemys = true;
            if (currentEnemysDie < auxCountEnemysDieForPassLevel)
            {
                if (OnEnableGeneratedEnemys != null)
                    OnEnableGeneratedEnemys(this);
            }
            else
            {
                enablePassLevel = true;
            }
        }
    }

    public bool GetEnablePassLevel()
    {
        return enablePassLevel;
    }
}
