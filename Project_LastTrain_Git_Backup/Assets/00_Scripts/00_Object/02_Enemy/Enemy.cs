using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public abstract class Enemy : MonoBehaviour
{
    public EnemyData enemyData;
    public abstract void OnAwake();
    public virtual void Init(EnemyData enemydt)
    {
        if (enemydt == null)
        {
            enemyData.maxHp = 5f;
            enemyData.moveSpeed = 3f;
            enemyData.chaseSpeed = 5f;
            enemyData.attackDistance = 1.5f;
            enemyData.findDistance = 5f;
            enemyData.attackSpeed = 0.25f;
        }
        enemyData = enemydt;
    }
    public abstract void OnUpdate();
    public abstract void OnLateUpdate();
    public virtual void SetEnemyPos(Vector3 pos)
    {
        transform.position = pos;
    }
    public abstract void OnDespawn();
}
