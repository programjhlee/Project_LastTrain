using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    public float hp;
    public float moveSpeed;
    public float chaseSpeed;
    public int coin;
}
