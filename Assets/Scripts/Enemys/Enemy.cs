using UnityEngine;
using UnityEngine.AI;
using System;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnDieEnemy;
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

    protected void CheckLifeOut(int sendEventLifeOut)
    {
        if (healthSystem.CheckDie())
        {
            if (OnDieEnemy != null)
                OnDieEnemy(this);

            fsmEnemy.SendEvent(sendEventLifeOut);
        }
    }
}
