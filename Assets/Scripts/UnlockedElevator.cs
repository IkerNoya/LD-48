using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockedElevator : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private Transform targetDoorLeft;
    [SerializeField] private Transform targetDoorRight;
    [SerializeField] private Transform targetOpenDoor;
    [SerializeField] private float speedMovement;
    [SerializeField] private string nameGameOverScene;

    void Update()
    {
        if (levelManager.GetEnablePassLevel() && transform.position.y < targetOpenDoor.position.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetOpenDoor.position, speedMovement * Time.deltaTime);
        }

        if (transform.position.y >= targetOpenDoor.position.y)
        {
            if(leftDoor.transform.position.x > targetDoorLeft.position.x)
                leftDoor.transform.position = Vector3.MoveTowards(transform.position, targetDoorLeft.position, speedMovement * Time.deltaTime);


            if (rightDoor.transform.position.x < targetDoorRight.position.x)
                rightDoor.transform.position = Vector3.MoveTowards(transform.position, targetDoorRight.position, speedMovement * Time.deltaTime);

        }
    }

    public void FinishGame()
    {
        SceneManager.LoadScene(nameGameOverScene);
    }
}
