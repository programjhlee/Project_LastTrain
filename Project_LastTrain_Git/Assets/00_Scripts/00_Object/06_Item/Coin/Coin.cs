using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coin : Item, IGravityAffected
{

    CollideChecker _CollideChecker;
    
    
    float _yVel = 10;
    bool _isActive;

    public float spawnDirX;

    public bool IsActive { get { return _isActive; } set { _isActive = value; } }
    public float YVel { get { return _yVel; } set { _yVel = value; } }

    public Transform TargetTransform { get { return transform; } }

    public CollideChecker CollideChecker { get { return _CollideChecker; } }


    public override void Init(ItemData itemData, Vector3 spawnPos)
    {
        base.Init(itemData, spawnPos);
        _CollideChecker = gameObject.AddComponent<CollideChecker>();
        spawnDirX = UnityEngine.Random.Range(-1f, 1f);
        GetItem += LootManager.Instance.IncreaseResource;
        GravityManager.Instance.AddGravityObj(this);
        IsActive = true;
    }

    public override void OnUpdate()
    {
        if (_CollideChecker.IsLanding)
        {
            spawnDirX = 0;
        }

        transform.position += new Vector3(spawnDirX, _yVel, 0) * Time.deltaTime;
        _CollideChecker.LandCheck();
    }

    public override void Clear()
    {
        ReleaseEvent();
        gameObject.SetActive(false);
        IsActive = false;
    }
    public void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            InvokeGetItem(this);
            Clear();
        }
    }
}
