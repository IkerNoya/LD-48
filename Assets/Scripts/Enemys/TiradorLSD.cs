using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiradorLSD : Enemy
{
    // Start is called before the first frame update

    enum TiradorLSD_STATES
    {
        Idle,
        SelectorAction,
        Teleport,
        Attack,
        Die,
        Count,
    }

    enum TiradorLSD_EVENTS
    {
        StartBehaviour,
        ChangeSelectorAction,
        ChangeTeleportState,
        ChangeAttackState,
        LifeOut,
        Count,
    }

    [SerializeField] private Transform spawnProjectiles;
    [SerializeField] private float damageBulletTirador;
    [SerializeField] private float speedBulletTirador;
    [SerializeField] private Bullet originalObject;
    [SerializeField] private float delayShoot;
    [SerializeField] private int minCountShootForSwitchSlectorActionState = 5;
    [SerializeField] private int maxCountShootForSwitchSlectorActionState = 5;
    [SerializeField] private int countShootForSwitchSlectorActionState;

    [SerializeField] private LayerMask layerPlayer;

    [SerializeField] private float heathRequestTeleport;

    [Range(0, 100)]
    [SerializeField] private float porcentageTeleportInShortLife = 65.0f;

    [SerializeField] private string tagGeneratorPosition = "GeneratorBasedMyPosition";
    [SerializeField] private List<PositionGeneratorBasedMyPosition> positionGeneratorsBasedMyPositions;
    [SerializeField] private float addHeightInTeleport;

    private float auxDelayShoot;
    Animator anim;

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
        fsmEnemy = new FSM((int)TiradorLSD_STATES.Count, (int)TiradorLSD_EVENTS.Count, (int)TiradorLSD_STATES.Idle);

        fsmEnemy.SetRelations((int)TiradorLSD_STATES.Idle, (int)TiradorLSD_STATES.SelectorAction, (int)TiradorLSD_EVENTS.StartBehaviour);

        fsmEnemy.SetRelations((int)TiradorLSD_STATES.SelectorAction, (int)TiradorLSD_STATES.Attack, (int)TiradorLSD_EVENTS.ChangeAttackState);
        fsmEnemy.SetRelations((int)TiradorLSD_STATES.SelectorAction, (int)TiradorLSD_STATES.Teleport, (int)TiradorLSD_EVENTS.ChangeTeleportState);

        fsmEnemy.SetRelations((int)TiradorLSD_STATES.Attack, (int)TiradorLSD_STATES.SelectorAction, (int)TiradorLSD_EVENTS.ChangeSelectorAction);
        fsmEnemy.SetRelations((int)TiradorLSD_STATES.Teleport, (int)TiradorLSD_STATES.SelectorAction, (int)TiradorLSD_EVENTS.ChangeSelectorAction);
        fsmEnemy.SetRelations((int)TiradorLSD_STATES.Teleport, (int)TiradorLSD_STATES.Attack, (int)TiradorLSD_EVENTS.ChangeAttackState);

        fsmEnemy.SetRelations((int)TiradorLSD_STATES.Idle, (int)TiradorLSD_STATES.Die, (int)TiradorLSD_EVENTS.LifeOut);
        fsmEnemy.SetRelations((int)TiradorLSD_STATES.SelectorAction, (int)TiradorLSD_STATES.Die, (int)TiradorLSD_EVENTS.LifeOut);
        fsmEnemy.SetRelations((int)TiradorLSD_STATES.Attack, (int)TiradorLSD_STATES.Die, (int)TiradorLSD_EVENTS.LifeOut);
        fsmEnemy.SetRelations((int)TiradorLSD_STATES.Teleport, (int)TiradorLSD_STATES.Die, (int)TiradorLSD_EVENTS.LifeOut);
    }

    protected override void Start()
    {
        base.Start();

        auxDelayShoot = delayShoot;

        anim = GetComponent<Animator>();

        countShootForSwitchSlectorActionState = Random.Range(minCountShootForSwitchSlectorActionState, maxCountShootForSwitchSlectorActionState);

        GameObject[] positionGeneratorsBasedMyPositions_GO = GameObject.FindGameObjectsWithTag(tagGeneratorPosition);

        for (int i = 0; i < positionGeneratorsBasedMyPositions_GO.Length; i++)
        {
            positionGeneratorsBasedMyPositions.Add(positionGeneratorsBasedMyPositions_GO[i].GetComponent<PositionGeneratorBasedMyPosition>());
        }

        for (int i = 0; i < positionGeneratorsBasedMyPositions.Count; i++)
        {
            if (positionGeneratorsBasedMyPositions[i] == null)
            {
                positionGeneratorsBasedMyPositions.Remove(positionGeneratorsBasedMyPositions[i]);
            }
        }
    }

    void Update()
    {
        switch (fsmEnemy.GetCurrentState())
        {
            case (int)TiradorLSD_STATES.Idle:
                Idle();
                break;
            case (int)TiradorLSD_STATES.SelectorAction:
                SelectorAction();
                break;
            case (int)TiradorLSD_STATES.Attack:
                Attack();
                break;
            case (int)TiradorLSD_STATES.Teleport:
                Teleport();
                break;
            case (int)TiradorLSD_STATES.Die:
                Die();
                break;
        }
    }

    private void Idle()
    {
        if (startBehaviour)
        {
            anim.SetBool("Idle", true);
            fsmEnemy.SendEvent((int)TiradorLSD_EVENTS.StartBehaviour);
        }
    }

    private void SelectorAction()
    {
        if (healthSystem.GetLife() <= heathRequestTeleport)
        {
            float porcentage = Random.Range(0, 100);
            if (porcentage <= porcentageTeleportInShortLife)
            {
                fsmEnemy.SendEvent((int)TiradorLSD_EVENTS.ChangeTeleportState);
            }
            else
            {
                fsmEnemy.SendEvent((int)TiradorLSD_EVENTS.ChangeAttackState);
            }
        }
        else
        {
            fsmEnemy.SendEvent((int)TiradorLSD_EVENTS.ChangeAttackState);
        }

        CheckLifeOut((int)TiradorLSD_EVENTS.LifeOut);
    }

    private void Attack()
    {
        transform.LookAt(new Vector3(CurrentTarget.position.x, transform.position.y, CurrentTarget.position.z));
        spawnProjectiles.LookAt(CurrentTarget);

        RaycastHit hit;

        if (Physics.Raycast(spawnProjectiles.position, spawnProjectiles.forward, out hit, Mathf.Infinity, layerPlayer))
        {
            if (delayShoot > 0)
            {
                delayShoot = delayShoot - Time.deltaTime;
            }
            else
            {
                anim.SetTrigger("Attack");
                
            }
        }

        CheckLifeOut((int)TiradorLSD_EVENTS.LifeOut);
    }

    private void Teleport()
    {
        ChangePositionTeleport();
        fsmEnemy.SendEvent((int)TiradorLSD_EVENTS.ChangeAttackState);
        CheckLifeOut((int)TiradorLSD_EVENTS.LifeOut);
    }
    
    private void Die()
    {
        anim.SetTrigger("Die");
        healthSystem.CheckDieEvent();
    }

    private void OnHitMe(Weapon weaponHitMe, Transform _transform)
    {
        if (transform == _transform)
        {
            healthSystem.SubstractLife(weaponHitMe.GetDamage());
        }
    }

    public void ChangePositionTeleport()
    {
        int indexPosition = Random.Range(0, positionGeneratorsBasedMyPositions.Count);
        transform.position = positionGeneratorsBasedMyPositions[indexPosition].GetPositionGenerated() + new Vector3(0, addHeightInTeleport, 0);
    }

    public void FireProjectile()
    {
        delayShoot = auxDelayShoot;
        Bullet bullet = Instantiate(originalObject.gameObject, spawnProjectiles.transform.position, spawnProjectiles.transform.rotation).GetComponent<Bullet>();
        bullet.SetDirection(bullet.transform.forward);
        bullet.SetDamage(damageBulletTirador);
        bullet.SetSpeed(speedBulletTirador);
        countShootForSwitchSlectorActionState--;
        if (countShootForSwitchSlectorActionState <= 0)
        {
            countShootForSwitchSlectorActionState = Random.Range(minCountShootForSwitchSlectorActionState, maxCountShootForSwitchSlectorActionState);
            fsmEnemy.SendEvent((int)TiradorLSD_EVENTS.ChangeSelectorAction);
        }
    }
}
