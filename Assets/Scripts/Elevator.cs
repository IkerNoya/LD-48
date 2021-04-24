using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] public GameObject leftDoor;
    [SerializeField] public GameObject rightDoor;

    [SerializeField] public GameObject moveTo;
    [SerializeField] public Vector3 initialPos;

    [SerializeField] public float speedUp;

    private float rangeToMoveDoor;
    private Vector3 limitDoorRightOpen;
    private Vector3 limitDoorLeftOpen;
    public enum ElevatorState
    {
        Hide,
        OnTransitionUP,
        OnTransitionDoors,
        InScene
    }
    public ElevatorState state;

    public void Awake()
    {
        initialPos = transform.position;
        rangeToMoveDoor = leftDoor.transform.localScale.x;
    }
    public void Update()
    {
        switch (state)
        {
            case ElevatorState.Hide:
                if(transform.position != initialPos)
                    transform.position = Vector3.MoveTowards(transform.position, initialPos, speedUp * Time.deltaTime);
                break;
            case ElevatorState.OnTransitionUP:
                if (transform.position != moveTo.transform.position)
                    transform.position = Vector3.MoveTowards(transform.position, moveTo.transform.position, speedUp * Time.deltaTime);
                else
                {
                    SetState(ElevatorState.OnTransitionDoors);
                    SetLimitsDoors();
                }
                break;
            case ElevatorState.OnTransitionDoors:
                if(leftDoor.transform.position != limitDoorLeftOpen && rightDoor.transform.position != limitDoorRightOpen)
                {
                    leftDoor.transform.position = 
                        Vector3.MoveTowards(leftDoor.transform.position, limitDoorLeftOpen, speedUp * Time.deltaTime);
                    rightDoor.transform.position =
                        Vector3.MoveTowards(rightDoor.transform.position, limitDoorRightOpen , speedUp * Time.deltaTime);
                }
                else
                    SetState(ElevatorState.InScene);
                break;
            case ElevatorState.InScene:
                break;
        }
    }
    public void SetLimitsDoors()
    {
        limitDoorRightOpen = new Vector3(rightDoor.transform.position.x + rangeToMoveDoor, rightDoor.transform.position.y, rightDoor.transform.position.z);
        limitDoorLeftOpen = new Vector3(leftDoor.transform.position.x - rangeToMoveDoor, leftDoor.transform.position.y, leftDoor.transform.position.z);
    }
    public void MoveElevator()
    {
        if(state != ElevatorState.InScene)
            SetState(ElevatorState.OnTransitionUP);
    }
    public void HideElevator()
    {
        SetState(ElevatorState.Hide);
    }
    public void SetState(ElevatorState newState)
    {
        state = newState;
    }
}
