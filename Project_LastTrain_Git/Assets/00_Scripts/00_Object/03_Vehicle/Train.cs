using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Train : MonoBehaviour
{
    float _maxHp;
    float _curHp;
    TrainEventSystem _eventSystem;
    float curTime = 0;

    public event Action OnDamaged;
    public event Action OnFixed;
    public event Action OnTrainDestroy;
    public float MaxHp
    {
        get
        {
            return _maxHp;
        }
        private set
        {
            _maxHp = value;
        }
    }
    public float CurHp
    {
        get
        {
            return _curHp;
        }
        set
        {
            _curHp = value;
        }
    }

    void Start()
    {
        _maxHp = 100;
        _curHp = _maxHp;
        _eventSystem = GetComponent<TrainEventSystem>();
    }

    public void TakeDamage(float damage)
    {
        _curHp -= damage;
        OnDamaged?.Invoke();
        if (_curHp <= 0)
        {
            OnTrainDestroy?.Invoke();
            return;
        }
    }
    public void TakeFix(float fixAmount)
    {
        _curHp += fixAmount;
    
        OnFixed?.Invoke();
    }
}
