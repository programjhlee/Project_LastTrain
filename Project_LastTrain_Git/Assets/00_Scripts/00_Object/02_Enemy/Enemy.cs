using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class Enemy : MonoBehaviour, IAttackable,IGravityAffected
{
    public EnemyData enemyData;

    Transform _player;
    PlayerAction _playerAction;
    CollideChecker _CollideChecker;

    Vector3 _moveDir;
    LayerMask _playerLayer;

    float _yVel = 0;
    float stateTime;
    public event Action<EnemyData> OnDamage;
    public event Action<Enemy> OnEnemyDied;

    public enum EnemyState
    {
        None,
        Detect,
        Chase,
        Attack,
        Die,
    }

    EnemyState _enemyState;

    public float YVel { get { return _yVel; } set { _yVel = value; } }
    public Transform TargetTransform { get { return transform; } }
    public CollideChecker CollideChecker { get { return _CollideChecker; } }

    public void Awake()
    {
        _CollideChecker = GetComponent<CollideChecker>();
    }
    void OnDisable()
    {

        OnDamage = null;
        OnEnemyDied = null;
    }

    public void Init(EnemyData enemydt)
    {
        if(enemydt == null)
        {
            enemyData.maxHp = 5f;
            enemyData.moveSpeed= 3f;
            enemyData.chaseSpeed = 5f;
            enemyData.coin = 2;
            enemyData.attackDistance = 1.5f;
            enemyData.findDistance = 5f;
            enemyData.attackSpeed = 0.25f;
            enemyData.attackDistance = 1.5f;
        }

        enemyData = enemydt;
        enemyData.curHp = enemyData.maxHp;
        enemyData.findDistance = 5f;
        enemyData.attackSpeed = 0.25f;
        enemyData.attackDistance = 1.5f;

        _enemyState = EnemyState.Detect;
        _moveDir = Vector3.left;
        stateTime = 0;
    }

    public void OnUpdate()
    {
        _CollideChecker.LandCheck();

        if (transform.position.y < -20f)
        {
            _enemyState = EnemyState.None;
        }

        switch (_enemyState)
        {
            case EnemyState.None:
                gameObject.SetActive(false);
                break;

            case EnemyState.Detect:
                if (_CollideChecker.IsLanding && _CollideChecker.IsCliff)
                {
                    _moveDir = -_moveDir;
                }
                _CollideChecker.CliffCheck(_moveDir);
                
                transform.position += new Vector3(_moveDir.x * enemyData.moveSpeed, _yVel, 0) * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(_moveDir);

                RaycastHit hit;

                if (CollideChecker.CollideCheckRay(transform.forward, _playerLayer, enemyData.findDistance, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        _player = hit.transform;
                        _playerAction = hit.transform.GetComponent<PlayerAction>();
                    }
                }

                if(_player != null)
                {
                    _enemyState = EnemyState.Chase;
                }

                break;

            case EnemyState.Chase:
                _moveDir = new Vector3(_player.transform.position.x - transform.position.x, 0, _player.transform.position.z - transform.position.z);
                _moveDir.Normalize();
                transform.position += new Vector3(_moveDir.x * enemyData.chaseSpeed, _yVel, _moveDir.z) * Time.deltaTime;
                if (Vector3.Distance(transform.position, _player.transform.position) <= enemyData.attackDistance)
                {
                    _enemyState = EnemyState.Attack;
                }
                break;

            case EnemyState.Attack:

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
                OnEnemyDied?.Invoke(GetComponent<Enemy>());
                _enemyState = EnemyState.None;
                break;
        }
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
    public void TakeDamage(float damage ,Vector3 dir)
    {
        StartCoroutine(DamageProcess(dir));
        enemyData.curHp -= damage;
        if(enemyData.curHp <= 0)
        {
            _enemyState = EnemyState.Die;
        }
    }
}
