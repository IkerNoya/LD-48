using System;
using UnityEngine;

public class Perseguidor : Enemy
{
    public enum CurrentBehaviourPerseguidor
    {
        None,
        AttackTarget,
    }

    enum Perseguidor_STATES
    {
        Idle,
        InitPerseguidor,
        RunToTarget,
        AttackTarget,
        Die,
        Count,
    }

    enum Perseguidor_EVENTS
    {
        StartBehaviour,
        AssignedAttackTargetBehaviour,
        StartBehaviourAttack,
        InRangeAttack,
        OutRangeAttack,
        LifeOut,
        Count,
    }

    [SerializeField] private float speedRunToTarget;
    [SerializeField] private float damagePerseguidor;
    [SerializeField] private float rangeToAttack;
    [SerializeField] private float delayAttack;
    [SerializeField] private bool loockAtTargetInAttack;
    private float auxDelayAttack;

    [SerializeField] private Rigidbody rig;
    [SerializeField] private LayerMask layerPlayer;
    [SerializeField] private CurrentBehaviourPerseguidor currentBehaviourPerseguidor = CurrentBehaviourPerseguidor.None;

    public static event Action<float, Transform> OnDamagePerseguidor;

    void OnEnable()
    {
        Weapon.HitDamage += OnHitMe;
    }

    void OnDisable()
    {
        Weapon.HitDamage -= OnHitMe;
    }


    void Awake()
    {
        fsmEnemy = new FSM((int)Perseguidor_STATES.Count, (int)Perseguidor_EVENTS.Count, (int)Perseguidor_STATES.Idle);

        fsmEnemy.SetRelations((int)Perseguidor_STATES.Idle, (int)Perseguidor_STATES.InitPerseguidor, (int)Perseguidor_EVENTS.StartBehaviour);

        fsmEnemy.SetRelations((int)Perseguidor_STATES.InitPerseguidor, (int)Perseguidor_STATES.RunToTarget, (int)Perseguidor_EVENTS.AssignedAttackTargetBehaviour);

        fsmEnemy.SetRelations((int)Perseguidor_STATES.RunToTarget, (int)Perseguidor_STATES.AttackTarget, (int)Perseguidor_EVENTS.InRangeAttack);

        fsmEnemy.SetRelations((int)Perseguidor_STATES.AttackTarget, (int)Perseguidor_STATES.RunToTarget, (int)Perseguidor_EVENTS.OutRangeAttack);

        fsmEnemy.SetRelations((int)Perseguidor_STATES.Idle, (int)Perseguidor_STATES.Die, (int)Perseguidor_EVENTS.LifeOut);
        fsmEnemy.SetRelations((int)Perseguidor_STATES.InitPerseguidor, (int)Perseguidor_STATES.Die, (int)Perseguidor_EVENTS.LifeOut);
        fsmEnemy.SetRelations((int)Perseguidor_STATES.RunToTarget, (int)Perseguidor_STATES.Die, (int)Perseguidor_EVENTS.LifeOut);
        fsmEnemy.SetRelations((int)Perseguidor_STATES.AttackTarget, (int)Perseguidor_STATES.Die, (int)Perseguidor_EVENTS.LifeOut);
    }

    protected override void Start()
    {
        base.Start();
        auxDelayAttack = delayAttack;
    }

    void Update()
    {
        switch (fsmEnemy.GetCurrentState())
        {
            case (int)Perseguidor_STATES.Idle:
                Idle(); //IDLE
                break;
            case (int)Perseguidor_STATES.InitPerseguidor:
                InitPerseguidor(); //ASIGNA COMPORTAMIENTO (PONELE ANIMACION IDLE)
                break;
            case (int)Perseguidor_STATES.RunToTarget:
                RunToTarget(); // CORRE HACIA EL JUGADOR
                break;
            case (int)Perseguidor_STATES.AttackTarget:
                AttackTarget(); //EJECUTA LA ANIMACION DE IDLE
                break;
            case (int)Perseguidor_STATES.Die:
                Die(); // C MUERE EL ENEMIGO
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
        }
        CheckLifeOut((int)Perseguidor_EVENTS.LifeOut);
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
        CheckLifeOut((int)Perseguidor_EVENTS.LifeOut);
    }

    private void AttackTarget()
    {
        navMeshAgent.speed = 0;
        navMeshAgent.acceleration = 1000;
        navMeshAgent.isStopped = true;
        //navMeshAgent.enabled = false;

        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;

        if(loockAtTargetInAttack)
            transform.LookAt(new Vector3(CurrentTarget.position.x, transform.position.y , CurrentTarget.position.z));

        if (delayAttack > 0)
        {
            delayAttack = delayAttack - Time.deltaTime;
        }
        else
        {
            delayAttack = auxDelayAttack;

            Vector3 scale = new Vector3(rangeToAttack + 1, 0, rangeToAttack + 1);

            Collider[] collidersOverlap = Physics.OverlapBox(transform.position, transform.localScale + scale, Quaternion.identity, layerPlayer);

            for (int i = 0; i < collidersOverlap.Length; i++)
            {
                if (collidersOverlap[i] != null)
                {
                    if(OnDamagePerseguidor != null)
                        OnDamagePerseguidor(damagePerseguidor, collidersOverlap[i].transform);
                }
            }
        }
        float distance = Vector3.Distance(transform.position, CurrentTarget.position);
        if (distance > rangeToAttack)
        {
            fsmEnemy.SendEvent((int)Perseguidor_EVENTS.OutRangeAttack);
        }
        CheckLifeOut((int)Perseguidor_EVENTS.LifeOut);
    }

    private void Die()
    {
        healthSystem.CheckDieEvent();
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
