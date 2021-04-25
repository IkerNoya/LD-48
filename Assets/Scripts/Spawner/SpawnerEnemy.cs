using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    [SerializeField] private List<Spawn> spawnPositions;
    [SerializeField] private bool inmediatedGeneratedInStart = true;
    [Range(1, 100)]
    [SerializeField] private float porcentageSpawn;
    [SerializeField] private float delaySpawnEnemy;
    private float auxDelaySpawnEnemy;
    [SerializeField] private float addHeightSpawn;
    
    [SerializeField] private LevelManager levelManager;

    private int countEnemysGenerated = 0;
    private int totalEnemysGenerated;


    private static SpawnerEnemy instance;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void OnEnable()
    {
        LevelManager.OnEnableGeneratedEnemys += OnEnableGeneratedEnemy;
    }

    void OnDisable()
    {
        LevelManager.OnEnableGeneratedEnemys -= OnEnableGeneratedEnemy;
    }

    void Start()
    {
        levelManager = LevelManager.instance;

        auxDelaySpawnEnemy = delaySpawnEnemy;

        if (inmediatedGeneratedInStart)
            delaySpawnEnemy = 0;

        AssignedCountEnemysGenerated();
    }

    private void AssignedCountEnemysGenerated()
    {
        totalEnemysGenerated = levelManager.countEnemysDieForPassLevel;
        countEnemysGenerated = (int)GetCountEnemysGenerated();
        levelManager.needEnemysDieForNextGenerationsEnemys = countEnemysGenerated;
    }

    private float GetCountEnemysGenerated()
    {
        return (porcentageSpawn / 100) * totalEnemysGenerated;
    }

    void Update()
    {
        CheckSpawn();
    }

    public void CheckSpawn()
    {
        if (countEnemysGenerated > 0)
        {
            if (delaySpawnEnemy > 0)
            {
                delaySpawnEnemy = delaySpawnEnemy - Time.deltaTime;
            }
            else
            {
                delaySpawnEnemy = auxDelaySpawnEnemy;
                ChooseSpawn();
                countEnemysGenerated--;
            }
        }
        else
        {
            levelManager.enableGeneratedEnemys = false;
        }
    }

    private void OnEnableGeneratedEnemy(LevelManager lm)
    {
        AssignedCountEnemysGenerated();
    }

    private void ChooseSpawn()
    {
        int index = Random.Range(0, spawnPositions.Count);
        Spawn choosedSpawn = spawnPositions[index];

        Vector3 position = choosedSpawn.transform.position + choosedSpawn.myPositionGeneratorBasedMyPosition.GetPositionGenerated() + new Vector3(0, addHeightSpawn, 0);

        if (choosedSpawn.CheckEnableGeneratorEnemy())
        {
            Instantiate(choosedSpawn.enemyPrefab, position, Quaternion.identity);
            if (!choosedSpawn.ilimitedSpawnEnemys)
                choosedSpawn.AddCurrentEnemysGenerated();
        }
        else
        {
            for (int i = 0; i < spawnPositions.Count; i++)
            {
                if (spawnPositions[i].CheckEnableGeneratorEnemy())
                {
                    Instantiate(choosedSpawn.enemyPrefab, position, Quaternion.identity);
                    if (!choosedSpawn.ilimitedSpawnEnemys)
                        choosedSpawn.AddCurrentEnemysGenerated();
                }
            }
        }
    }
}
