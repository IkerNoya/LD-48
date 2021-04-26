using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public bool ilimitedSpawnEnemys;
    private int currentEnemysGenerated = 0;
    public int limitSpawn;
    public PositionGeneratorBasedMyPosition myPositionGeneratorBasedMyPosition;
    public GameObject enemyPrefab;

    public void AddCurrentEnemysGenerated()
    {
        currentEnemysGenerated++;
    }
    public bool CheckEnableGeneratorEnemy()
    {
        if (ilimitedSpawnEnemys)
            return true;

        if (currentEnemysGenerated < limitSpawn)
            return true;

        return false;
    }
}
