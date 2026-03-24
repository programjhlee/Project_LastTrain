using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : GroundEnemy, IAttackable, IDroppedItem
{
    Transform _player;
    PlayerAction _playerAction;
    EnemyUIController _enemyUIController;
    ZombieSoundController _zombieSoundController;
    ZombieAnim _anim;

    Vector3 _moveDir;
    LayerMask _playerLayer;
    public float Maxhp { get; set; }
    public float Curhp { get; set; }
    public Vector3 DropPoint { get; set; }

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

    public event Action OnChase;
    public event Action OnDamaged;
    public event Action OnAttack;
    public event Action<Zombie> OnDied;

    public override void OnAwake()
    {
        base.OnAwake();
        _playerLayer = LayerMask.GetMask("Player");
        _enemyUIController = GetComponent<EnemyUIController>();
        _anim = GetComponent<ZombieAnim>();
        _zombieSoundController = GetComponent<ZombieSoundController>();
    }
    public override void Init(EnemyData enemydt)
    {
        base.Init(enemydt);

        IsActive = true;

        Curhp = enemyData.maxHp;
        Maxhp = enemyData.maxHp;

        enemyData.findDistance = 5f;
        enemyData.attackSpeed = 2f;
        enemyData.attackDistance = 1.5f;

        _enemyState = EnemyState.Detect;
        _moveDir = Vector3.left;
        stateTime = 0;

        gameObject.layer = LayerMask.NameToLayer("Enemy");

        _enemyUIController.Init();
        _anim.Init();
        _zombieSoundController.Init();

        OnDamaged += _enemyUIController.SetValueBarRatio;
        OnDied += LootManager.Instance.DropItemAt;
    }
    void OnDisable()
    {
        OnDamaged = null;
        OnDied = null;
    }

    public override void OnUpdate()
    {
        CollideChecker.LandCheck();

        if (transform.position.y < -20f)
        {
            _enemyState = EnemyState.None;
            OnDespawn();
        }

        switch (_enemyState)
        {
            case EnemyState.None:
                break;

            case EnemyState.Detect:
                _zombieSoundController.OnSoundUpdate();
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
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        _player = hit.transform;
                        _playerAction = hit.transform.GetComponent<PlayerAction>();
                    }
                }

                if (_player != null)
                {
                    OnChase?.Invoke();
                    _enemyState = EnemyState.Chase;


                }

                break;

            case EnemyState.Chase:
                _zombieSoundController.OnSoundUpdate();
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
                _zombieSoundController.OnSoundUpdate();
                stateTime += Time.deltaTime;
                if (stateTime > 1f/enemyData.attackSpeed)
                {
                    stateTime = 0;
                    OnAttack?.Invoke();
                    _playerAction.TakeDamage(_moveDir);
                    if (Vector3.Distance(transform.position, _player.transform.position) > enemyData.attackDistance)
                    {
                        stateTime = 0;
                        _enemyState = EnemyState.Chase;
                    }
                }
                break;

            case EnemyState.Die:
                _zombieSoundController.OnSoundUpdate();
                transform.position += new Vector3(0, YVel, 0) * Time.deltaTime;
                if (CollideChecker.IsLanding)
                {
                    stateTime += Time.deltaTime;
                }
                if (stateTime >= 3f)
                {
                    OnDespawn();
                }
                break;
        }
    }

    public override void OnLateUpdate()
    {
        _enemyUIController.UpdateUIPos();
    }

    public override void OnDespawn()
    {
        IsActive = false;
        transform.position = Vector3.zero;
        _enemyUIController.HideUIHUD();
        _enemyState = EnemyState.None;
        _zombieSoundController.ResetSoundController();
        OnChase = null;
        OnDamaged = null;
        OnAttack = null;
        OnDied = null;
        gameObject.SetActive(false);
    }

    public void TakeDamage(float damage, Vector3 dir)
    {
        if (_enemyState == EnemyState.Die || _enemyState == EnemyState.None)
        {
            return;
        }

        StartCoroutine(DamageProcess(dir));

        _moveDir = -dir;
        Curhp -= damage;

        OnDamaged?.Invoke();
        if (Curhp <= 0)
        {
            DropPoint = transform.position;
            _enemyUIController.HideUIHUD();
            gameObject.layer = LayerMask.NameToLayer("Dead");
            OnDied?.Invoke(this);
            _enemyState = EnemyState.Die;
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


}
