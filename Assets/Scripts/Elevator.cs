using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] public GameObject leftDoor;
    [SerializeField] public GameObject rightDoor;

    [SerializeField] public GameObject moveTo;

    [SerializeField] public Vector3 initialPos;

    [SerializeField]
    public enum ElevatorState
    {
        Hide,
        OnTransition,
        InScene
    }
    public ElevatorState state;

    private void Start()
    {
        initialPos = transform.position;
    }
    void Update()
    {
        
    }

    public void MoveElevator()
    {

    }
}
