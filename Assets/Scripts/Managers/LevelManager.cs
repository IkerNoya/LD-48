using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelManager : MonoBehaviour
{
    [SerializeField] private bool useSingeltone;
    public static event Action<LevelManager> OnEnableGeneratedEnemys;

    public static LevelManager instance;

    [HideInInspector] public bool enableGeneratedEnemys;

    public int countEnemysDieForPassLevel;

    private int auxCountEnemysDieForPassLevel;

    [HideInInspector] public int needEnemysDieForNextGenerationsEnemys;

    private int currentEnemysDie = 0;

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
        if (useSingeltone)
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }
    }
    void Start()
    {
        enablePassLevel = false;
        auxCountEnemysDieForPassLevel = countEnemysDieForPassLevel;
    }

    private void OnEnemyDie(Enemy e)
    {
        currentEnemysDie++;

        if (needEnemysDieForNextGenerationsEnemys > 0)
            needEnemysDieForNextGenerationsEnemys--;

        if(needEnemysDieForNextGenerationsEnemys <= 0)
        {
            //Debug.Log("ME MORI OwO");
            enableGeneratedEnemys = true;
            if (currentEnemysDie <= auxCountEnemysDieForPassLevel)
            {
                //Debug.Log("ME MORI UWU");
                 if(currentEnemysDie == auxCountEnemysDieForPassLevel)
                 {
                    enablePassLevel = true;
                 }
                if (OnEnableGeneratedEnemys != null)
                    OnEnableGeneratedEnemys(this);
            }
        }
    }

    public bool GetEnablePassLevel()
    {
        return enablePassLevel;
    }
}
