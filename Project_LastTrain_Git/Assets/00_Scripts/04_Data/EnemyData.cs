using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    public float curHp;
    public float maxHp;
    public float moveSpeed;
    public float chaseSpeed;
    public int coin;
    public float findDistance;
    public float attackDistance;
    public float attackSpeed;
}
