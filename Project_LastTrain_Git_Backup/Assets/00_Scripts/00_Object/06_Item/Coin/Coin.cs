using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class Coin : Item, IGravityAffected
{
    [SerializeField] AudioClip _coinDropSound;
    [SerializeField] AudioClip _coinGetSound;
    [SerializeField] GameObject _coinGetEffect;
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
        _CollideChecker = GetComponent<CollideChecker>();
        CollideChecker.Col.enabled = true;
        base.Init(itemData, spawnPos);
        SoundManager.Instance.PlaySFX(_coinDropSound);
        spawnDirX = UnityEngine.Random.Range(-1f, 1f);
        GetItem += LootManager.Instance.IncreaseResource;
        GravityManager.Instance.AddGravityObj(this);
        _yVel = 10;
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
        IsActive = false;
        gameObject.SetActive(false);
    }
    public void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            CollideChecker.Col.enabled = false;
            SoundManager.Instance.PlaySFX(_coinGetSound,0.5f);
            transform.position = coll.transform.position;
            Instantiate(_coinGetEffect, transform.position, Quaternion.identity);
            transform.DOMoveY(0.5f, 0.2f).SetLoops(2,LoopType.Yoyo).OnComplete(()=>Clear());
            InvokeGetItem(this);
        }
    }
}
