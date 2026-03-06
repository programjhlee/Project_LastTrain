using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : GroundEnemy, IAttackable
{
    Transform _player;
    Collider _enemyCol;
    PlayerAction _playerAction;
    EnemyUIController _enemyUIController;
    ZombieAnim _anim;

    Vector3 _moveDir;
    LayerMask _playerLayer;
    public float Maxhp { get; set; }
    public float Curhp { get; set; }

    float stateTime;
    public enum EnemyState
    {
        None,
        Detect,
        Chase,
        Attack,
        Die,
    }

    EnemyState _enemyState;

    public event Action OnDamaged;
    public event Action OnAttack;
    public event Action<Enemy> OnDied;

    public override void Awake()
    {
        base.Awake();
        _playerLayer = LayerMask.GetMask("Player");
        _enemyUIController = GetComponent<EnemyUIController>();
        _anim = GetComponent<ZombieAnim>();
        _enemyCol = GetComponent<Collider>();
    }
    public override void Init(EnemyData enemydt)
    {
        if (enemydt == null)
        {
            enemyData.maxHp = 5f;
            enemyData.moveSpeed = 3f;
            enemyData.chaseSpeed = 5f;
            enemyData.coin = 2;
            enemyData.attackDistance = 1.5f;
            enemyData.findDistance = 5f;
            enemyData.attackSpeed = 0.25f;
            enemyData.attackDistance = 1.5f;
        }

        base.Init(enemydt);
        IsActive = true;
        Curhp = enemyData.maxHp;
        Maxhp = enemyData.maxHp;
        enemyData.findDistance = 5f;
        enemyData.attackSpeed = 0.25f;
        enemyData.attackDistance = 1.5f;
        
        _enemyUIController.Init();
        _anim.ZombieAnimInit();

        _enemyCol.enabled = true;
        _enemyState = EnemyState.Detect;
        _moveDir = Vector3.left;
        stateTime = 0;
        
        OnDamaged += _enemyUIController.SetValueBarRatio;
        OnDied += LootManager.Instance.DropCoinAt;
    }
    void OnDisable()
    {
        OnDamaged = null;
        OnDied = null;
    }

    public override void OnUpdate()
    {
        Debug.Log($"IsLanding : {CollideChecker.IsLanding} , IsActive : {IsActive}");
        CollideChecker.LandCheck();
        _enemyUIController.UpdateUIPos();

        if (transform.position.y < -20f)
        {
            _enemyState = EnemyState.None;
        }

        switch (_enemyState)
        {
            case EnemyState.None:
                transform.position += new Vector3(0, YVel, 0) * Time.deltaTime;
                if (CollideChecker.IsLanding)
                {
                    stateTime += Time.deltaTime;
                }
                if(stateTime >= 3f)
                {
                    Clear();
                }
                break;

            case EnemyState.Detect:
                if (CollideChecker.IsLanding && CollideChecker.IsCliff)
                {
                    _moveDir = -_moveDir;
                }
                CollideChecker.CliffCheck(_moveDir);

                transform.position += new Vector3(_moveDir.x * enemyData.moveSpeed, YVel, 0) * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(_moveDir);

                RaycastHit hit;

                if (CollideChecker.CollideCheckRay(transform.forward, _playerLayer, enemyData.findDistance, out hit))
                {
                    Debug.Log(hit.collider.gameObject);
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        Debug.Log("플레이어 발견!");
                        _player = hit.transform;
                        _playerAction = hit.transform.GetComponent<PlayerAction>();
                    }
                }

                if (_player != null)
                {
                    _enemyState = EnemyState.Chase;
                }

                break;

            case EnemyState.Chase:
                _moveDir = new Vector3(_player.transform.position.x - transform.position.x, 0, _player.transform.position.z - transform.position.z);
                _moveDir.Normalize();
                transform.position += new Vector3(_moveDir.x * enemyData.chaseSpeed, YVel, _moveDir.z) * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(_moveDir);
                if (Vector3.Distance(transform.position, _player.transform.position) <= enemyData.attackDistance)
                {
                    _enemyState = EnemyState.Attack;
                }
                break;

            case EnemyState.Attack:

                OnAttack?.Invoke();
                stateTime += Time.deltaTime;
                if (stateTime > enemyData.attackSpeed)
                {
                    stateTime = 0;
                    _playerAction.TakeDamage(_moveDir);
                }
                if (Vector3.Distance(transform.position, _player.transform.position) > enemyData.attackDistance)
                {
                    stateTime = 0;
                    _enemyState = EnemyState.Chase;
                }
                break;

            case EnemyState.Die:
                Die();
                break;
        }
    }

    public override void OnLateUpdate()
    {
        _enemyUIController.UpdateUIPos();
    }
    IEnumerator DamageProcess(Vector3 dir)
    {
        float attackForce = 10f;
        float curTime = 0;
        while (curTime <= 0.1f)
        {
            curTime += Time.deltaTime;
            transform.position += new Vector3(dir.x, 0.4f, 0) * attackForce * Time.deltaTime;
            yield return null;
        }
        curTime = 0;
    }
    public void TakeDamage(float damage, Vector3 dir)
    {
        StartCoroutine(DamageProcess(dir));
        _moveDir = -dir;
        Curhp -= damage;
        OnDamaged?.Invoke();
        if (Curhp <= 0)
        {
            _enemyState = EnemyState.Die;
        }
    }
    public void Die()
    {
        OnDied?.Invoke(this);
        _enemyUIController.HideUIHUD();
        _enemyState = EnemyState.None;
    }

    public override void Clear()
    {
        IsActive = false;
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
    }
}
