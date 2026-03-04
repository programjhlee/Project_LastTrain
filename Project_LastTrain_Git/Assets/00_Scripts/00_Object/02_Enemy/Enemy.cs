using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public abstract class Enemy : MonoBehaviour
{
    public EnemyData enemyData;

    public virtual void Init(EnemyData enemydt)
    {
        enemyData = enemydt;
    }
    public abstract void OnUpdate();
    public virtual void SetEnemyPos(Vector3 pos)
    {
        transform.position = pos;
    }

    public abstract void Clear();
}
