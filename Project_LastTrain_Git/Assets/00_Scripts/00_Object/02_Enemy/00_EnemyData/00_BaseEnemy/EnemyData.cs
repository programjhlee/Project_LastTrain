using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    public int enemyID;
    public string enemyName;
    public float maxHp;
    public float moveSpeed;
    public float chaseSpeed;
    public float findDistance;
    public float attackDistance;
    public float attackSpeed;
    public string prefabAddress;
}
