using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedDoor : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Transform targetOpenDoor;
    [SerializeField] private float speedMovement;

    void Update()
    {
        if (levelManager.GetEnablePassLevel())
        {
            transform.position = Vector3.MoveTowards(transform.position, targetOpenDoor.position, speedMovement * Time.deltaTime);
        }

        if (transform.position.y < targetOpenDoor.position.y)
        {
            Destroy(gameObject);
        }
    }
}
