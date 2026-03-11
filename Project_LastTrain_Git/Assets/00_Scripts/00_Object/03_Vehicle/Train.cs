using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Train : MonoBehaviour
{
    float _maxHp;
    float _curHp;

    public event Action<float> OnDamaged;
    public event Action<float> OnHpChanged;
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
        OnHpChanged?.Invoke(_curHp / _maxHp);
    }

    public void TakeDamage(float damage)
    {
        _curHp -= damage;
        OnDamaged?.Invoke(damage);
        OnHpChanged?.Invoke(_curHp/_maxHp);
        if (_curHp <= 0)
        {
            OnTrainDestroy?.Invoke();
            return;
        }
    }
    public void TakeFix(float fixAmount)
    {
        _curHp += fixAmount;
        OnHpChanged?.Invoke(_curHp / _maxHp);
    }
}
