using UnityEngine;
using UnityEngine.AI;
using System;

public class Enemy : MonoBehaviour
{
    protected FSM fsmEnemy;
    [SerializeField] protected bool startBehaviour = false;

    [SerializeField] protected NavMeshAgent navMeshAgent;

    [SerializeField] protected HealthSystem healthSystem;

    [SerializeField] protected string nameTagTarget;
    [SerializeField] protected Transform CurrentTarget;

    public void SetStartBehaviour(bool value) => startBehaviour = value;

    public bool GetStartBehaviour() { return startBehaviour; }

    protected virtual void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag(nameTagTarget);
        CurrentTarget = go.transform;
    }

}
