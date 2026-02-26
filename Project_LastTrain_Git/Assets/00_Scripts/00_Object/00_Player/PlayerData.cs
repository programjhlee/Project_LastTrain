using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName ="Create PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] int _level;
    [SerializeField] float _attackPower;
    [SerializeField] float _fixPower;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _jumpForce;
    [SerializeField] float _rollingSpeed;
    public int Level { get { return _level; } set { _level = value; } }
    public float AttackPower { get { return _attackPower; } set { _attackPower = value; } }
    public float FixPower { get { return _fixPower; } set { _fixPower = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _fixPower = value; } }
    public float JumpForce { get { return _jumpForce; } set { _jumpForce = value; } }
    public float RollingSpeed { get { return _rollingSpeed; } set { _rollingSpeed = value; } }
}
