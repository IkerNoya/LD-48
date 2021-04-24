using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    [SerializeField] private List<Spawn> spawnPositions;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<GameObject> enemys;

    private void Awake()
    {
        enemys.Clear();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            ChooseSpawn();
        }
    }
    private void ChooseSpawn()
    {
        Spawn choosedSpawn = spawnPositions[Random.Range(0, spawnPositions.Count)];
        float newPosX = Random.Range(choosedSpawn.GetMaxX(), choosedSpawn.GetMinX());
        float newPosZ = Random.Range(choosedSpawn.GetMaxZ(), choosedSpawn.GetMinZ());

        enemys.Add(Instantiate(enemyPrefab, choosedSpawn.transform.position + new Vector3(newPosX, 1, newPosZ), Quaternion.identity));
    }
}
