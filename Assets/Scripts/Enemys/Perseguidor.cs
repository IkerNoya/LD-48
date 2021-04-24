using System;
using UnityEngine;

public class Perseguidor : Enemy
{
    [SerializeField] private float speedWalkAroundTheTarget;
    [SerializeField] private float speedRunToTarget;
    [SerializeField] private float speedZigZagToTarget;
    [SerializeField] private float damagePerseguidor;
    [SerializeField] private float rangeToAttack;
    [SerializeField] private float delayAttack;
    private float auxDelayAttack;

    [SerializeField] private LayerMask layerPlayer;
    [SerializeField] private float rangeToTargetWalkAroundTheTargert;
    [SerializeField] private float rangeModifyAssignedWaypoint;
    [SerializeField] private float delayWaitInWalkAroundTheTarget;
    [SerializeField] private float magnitudeWaypointsWalkAroundTheTarget = 1.0f;
    private float auxDelayWaitInWalkAroundTheTarget;

    [SerializeField] private CurrentBehaviourPerseguidor currentBehaviourPerseguidor = CurrentBehaviourPerseguidor.None;
    [SerializeField] private WalkAroundTheTargetSTATES walkAroundTheTargetSTATES = WalkAroundTheTargetSTATES.None;

    void OnEnable()
    {
        Weapon.HitDamage += OnHitMe;
    }

    void OnDisable()
    {
        Weapon.HitDamage -= OnHitMe;
    }

    public enum CurrentBehaviourPerseguidor
    {
        None,
        PatrolInRange,
        AttackTarget,
    }

    enum WalkAroundTheTargetSTATES
    {
        None,
        AssignedWaypoint,
        GoToWaypoint,
        Wait,
    }

    enum Perseguidor_STATES
    {
        Idle,
        InitPerseguidor,
        WalkAroundTheTarget,
        RunToTarget,
        ZigZagToTarget,
        AttackTarget,
        Die,
        Count,
    }

    enum Perseguidor_EVENTS
    {
        StartBehaviour,
        AssignedPatrolInRangeBehaviour,
        AssignedAttackTargetBehaviour,
        StartBehaviourAttack,
        ZigZagEvent,
        InRangeAttack,
        OutRangeAttack,
        LifeOut,
        Count,
    }

    void Awake()
    {
        fsmEnemy = new FSM((int)Perseguidor_STATES.Count, (int)Perseguidor_EVENTS.Count, (int)Perseguidor_STATES.Idle);

        fsmEnemy.SetRelations((int)Perseguidor_STATES.Idle, (int)Perseguidor_STATES.InitPerseguidor, (int)Perseguidor_EVENTS.StartBehaviour);

        fsmEnemy.SetRelations((int)Perseguidor_STATES.InitPerseguidor, (int)Perseguidor_STATES.WalkAroundTheTarget, (int)Perseguidor_EVENTS.AssignedPatrolInRangeBehaviour);
        fsmEnemy.SetRelations((int)Perseguidor_STATES.InitPerseguidor, (int)Perseguidor_STATES.RunToTarget, (int)Perseguidor_EVENTS.AssignedAttackTargetBehaviour);

        fsmEnemy.SetRelations((int)Perseguidor_STATES.WalkAroundTheTarget, (int)Perseguidor_STATES.RunToTarget, (int)Perseguidor_EVENTS.StartBehaviourAttack);

        fsmEnemy.SetRelations((int)Perseguidor_STATES.RunToTarget, (int)Perseguidor_STATES.ZigZagToTarget, (int)Perseguidor_EVENTS.ZigZagEvent);
        fsmEnemy.SetRelations((int)Perseguidor_STATES.RunToTarget, (int)Perseguidor_STATES.AttackTarget, (int)Perseguidor_EVENTS.InRangeAttack);

        fsmEnemy.SetRelations((int)Perseguidor_STATES.ZigZagToTarget, (int)Perseguidor_STATES.AttackTarget, (int)Perseguidor_EVENTS.InRangeAttack);

        fsmEnemy.SetRelations((int)Perseguidor_STATES.AttackTarget, (int)Perseguidor_STATES.RunToTarget, (int)Perseguidor_EVENTS.OutRangeAttack);

        fsmEnemy.SetRelations((int)Perseguidor_STATES.Idle, (int)Perseguidor_STATES.Die, (int)Perseguidor_EVENTS.LifeOut);
        fsmEnemy.SetRelations((int)Perseguidor_STATES.InitPerseguidor, (int)Perseguidor_STATES.Die, (int)Perseguidor_EVENTS.LifeOut);
        fsmEnemy.SetRelations((int)Perseguidor_STATES.WalkAroundTheTarget, (int)Perseguidor_STATES.Die, (int)Perseguidor_EVENTS.LifeOut);
        fsmEnemy.SetRelations((int)Perseguidor_STATES.RunToTarget, (int)Perseguidor_STATES.Die, (int)Perseguidor_EVENTS.LifeOut);
        fsmEnemy.SetRelations((int)Perseguidor_STATES.ZigZagToTarget, (int)Perseguidor_STATES.Die, (int)Perseguidor_EVENTS.LifeOut);
        fsmEnemy.SetRelations((int)Perseguidor_STATES.AttackTarget, (int)Perseguidor_STATES.Die, (int)Perseguidor_EVENTS.LifeOut);
    }

    void Start()
    {
        auxDelayAttack = delayAttack;
        auxDelayWaitInWalkAroundTheTarget = delayWaitInWalkAroundTheTarget;
    }

    void Update()
    {
        switch (fsmEnemy.GetCurrentState())
        {
            case (int)Perseguidor_STATES.Idle:
                Idle();
                break;
            case (int)Perseguidor_STATES.InitPerseguidor:
                InitPerseguidor();
                break;
            case (int)Perseguidor_STATES.WalkAroundTheTarget:
                WalkAroundTheTarget();
                break;
            case (int)Perseguidor_STATES.RunToTarget:
                RunToTarget();
                break;
            case (int)Perseguidor_STATES.ZigZagToTarget:
                ZigZagToTarget();
                break;
            case (int)Perseguidor_STATES.AttackTarget:
                AttackTarget();
                break;
            case (int)Perseguidor_STATES.Die:
                Die();
                break;
        }
    }
    
    private void Idle()
    {
        if (startBehaviour)
        {
            fsmEnemy.SendEvent((int)Perseguidor_EVENTS.StartBehaviour);
        }
    }

    private void InitPerseguidor()
    {
        switch (currentBehaviourPerseguidor)
        {
            case CurrentBehaviourPerseguidor.AttackTarget:
                fsmEnemy.SendEvent((int)Perseguidor_EVENTS.AssignedAttackTargetBehaviour);
                break;
            case CurrentBehaviourPerseguidor.PatrolInRange:
                fsmEnemy.SendEvent((int)Perseguidor_EVENTS.AssignedPatrolInRangeBehaviour);
                break;
        }
    }

    private void WalkAroundTheTarget()
    {
        switch (currentBehaviourPerseguidor)
        {
            case CurrentBehaviourPerseguidor.AttackTarget:

                break;
            case CurrentBehaviourPerseguidor.PatrolInRange:

                switch (walkAroundTheTargetSTATES)
                {
                    case WalkAroundTheTargetSTATES.AssignedWaypoint:

                        break;
                    case WalkAroundTheTargetSTATES.GoToWaypoint:

                        break;
                    case WalkAroundTheTargetSTATES.Wait:
                       
                        break;
                }
                break;
        }
    }

    private void RunToTarget()
    {
        navMeshAgent.speed = speedRunToTarget;
        navMeshAgent.acceleration = speedRunToTarget * 2;
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(CurrentTarget.position);

        float distance = Vector3.Distance(transform.position, CurrentTarget.position);

        if (distance <= rangeToAttack)
        {
            fsmEnemy.SendEvent((int)Perseguidor_EVENTS.InRangeAttack);
        }
        CheckLifeOut();
    }

    private void AttackTarget()
    {
        if (delayAttack > 0)
        {
            delayAttack = delayAttack - Time.deltaTime;
        }
        else
        {
            delayAttack = auxDelayAttack;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 5, layerPlayer))
            {
                Debug.Log("Hice daño");
            }
        }
        float distance = Vector3.Distance(transform.position, CurrentTarget.position);
        if (distance > rangeToAttack)
        {
            fsmEnemy.SendEvent((int)Perseguidor_EVENTS.OutRangeAttack);
        }
        CheckLifeOut();
    }

    private void ZigZagToTarget(){}

    private void Die()
    {
        Destroy(gameObject);
    }

    private void CheckLifeOut()
    {
        if (healthSystem.CheckDie())
        {
            fsmEnemy.SendEvent((int)Perseguidor_EVENTS.LifeOut);
        }
    }

    private void OnHitMe(Weapon weaponHitMe, Transform _transform)
    {
        if (transform == _transform)
        {
            healthSystem.SubstractLife(weaponHitMe.GetDamage());
        }
    }

    public void SetCurrentBehaviourPerseguidor(CurrentBehaviourPerseguidor value) => currentBehaviourPerseguidor = value;

}
