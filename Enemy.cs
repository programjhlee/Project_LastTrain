using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyData enemyData;

    void Start()
    {

    }
    
    
    public void Init(EnemyData enemyDt)
    {
        enemyData = enemyDt;
        Debug.Log(enemyData.hp);
    }
    public void Enemy


}
