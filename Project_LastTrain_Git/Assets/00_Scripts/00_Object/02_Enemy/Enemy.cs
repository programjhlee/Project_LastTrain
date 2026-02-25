using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IAttackable,IGravityAffected
{
    PlayerAction player;

    LandChecker landChecker;
    public EnemyData enemyData;

    [SerializeField] float _findDistance;
    [SerializeField] float _attackDistance;
    [SerializeField] float _attackSpeed;
    [SerializeField] float _attackStateTime;

    Vector3 moveDir;
    float curHp;
    float yVelocity = 0;

    public event Action<Enemy> OnEnemyDied;

    public enum EnemyState
    {
        None,
        Move,
        Tracking,
        Attack,
        Die,
    }

    EnemyState enemyState;


    public void Awake()
    {
        landChecker = GetComponent<LandChecker>();
    }

    void OnEnable()
    {
        enemyState = EnemyState.Move;
        moveDir = Vector3.left;
        _attackStateTime = 0;
    
    }

    void OnDisable()
    {
        OnEnemyDied = null;
    }


    public void OnUpdate()
    {
        if(transform.position.y < -20f)
        {
            enemyState = EnemyState.None;
        }
        landChecker.LandCheck();
        switch (enemyState)
        {
            case EnemyState.None:
                gameObject.SetActive(false);
                
                break;
            case EnemyState.Move:
                if (landChecker.IsCliff)
                {
                    moveDir = -moveDir;
                }
                transform.position += new Vector3(moveDir.x * enemyData.moveSpeed, yVelocity, 0) * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(moveDir);
                landChecker.CliffCheck(moveDir);
                if (player != null)
                {
                    enemyState = EnemyState.Tracking;
                }
                SearchPlayer();
                break;
            case EnemyState.Tracking:
                moveDir = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);
                moveDir.Normalize();
                transform.position += new Vector3(moveDir.x * enemyData.chaseSpeed, yVelocity, moveDir.z) * Time.deltaTime;
                if (Vector3.Distance(transform.position, player.transform.position) <= _attackDistance)
                {
                    enemyState = EnemyState.Attack;
                }
                break;
            case EnemyState.Attack:
                Attack();
                if (Vector3.Distance(transform.position, player.transform.position) > _attackDistance)
                {
                    _attackStateTime = 0;
                    enemyState = EnemyState.Tracking;
                }
                break;
            case EnemyState.Die:
                OnEnemyDied?.Invoke(GetComponent<Enemy>());
                enemyState = EnemyState.None;
                break;
        }
    }


    public void SearchPlayer()
    {
        Ray ray = new Ray(transform.position , transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, transform.forward * _findDistance);
        if (Physics.Raycast(ray,out hit, _findDistance))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                player = hit.collider.gameObject.GetComponent<PlayerAction>();
            }
        }
    }

    public void Init(EnemyData enemydt)
    {
        enemyData = enemydt;
        curHp = enemyData.hp;
    }


    public void Attack()
    {
        _attackStateTime += Time.deltaTime;
        if(_attackStateTime > _attackSpeed)
        {
            _attackStateTime = 0;
            player.TakeDamage(moveDir);
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
        curHp -= damage;
        if(curHp <= 0)
        {
            enemyState = EnemyState.Die;
        }
    }

    public void AffectGravity(float gravity)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        if (!landChecker.IsLanding)
        {
            yVelocity -= gravity * Time.deltaTime;
        }
        else
        {
            if (yVelocity < 0)
            {
                yVelocity = 0;
                transform.position = new Vector3(transform.position.x, landChecker.GetLandYPos(), 0);
            }
        }
    }
}
