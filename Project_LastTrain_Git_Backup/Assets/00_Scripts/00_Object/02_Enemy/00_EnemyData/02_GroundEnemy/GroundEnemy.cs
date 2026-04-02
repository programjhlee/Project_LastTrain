using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundEnemy : Enemy, IGravityAffected
{
    CollideChecker _collideChecker;
    bool _isActive;
    float _yVel;
    public bool IsActive { get { return _isActive; } set { _isActive = value; } }
    public float YVel { get { return _yVel; } set { _yVel = value; } }
    public Transform TargetTransform { get { return transform; } }
    public CollideChecker CollideChecker { get { return _collideChecker; } }

    public override void OnAwake()
    {
        _collideChecker = GetComponent<CollideChecker>();
    }
    public override void Init(EnemyData enemydt)
    {
        base.Init(enemydt);
        GravityManager.Instance.AddGravityObj(this);
    }
}
