using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Train : MonoBehaviour
{
    TrainSound _trainSound;

    float _maxHp;
    float _curHp;
    bool _isRunning = false;


    public event Action<float> OnDamaged;
    public event Action<float> OnHpChanged;
    public event Action OnTrainDestroy;


    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
        }
    }
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
    void Awake()
    {
        _trainSound = GetComponent<TrainSound>();
    }


    void Start()
    {
        _maxHp = 100;
        _curHp = _maxHp;
        OnHpChanged?.Invoke(_curHp / _maxHp);
        GameManager.Instance.OnTutorialStart += StartRunning;
        GameManager.Instance.OnStageClear += StopRunning;
        GameManager.Instance.OnStageStart += StartRunning;
    }

    public void OnDisable()
    {
        GameManager.Instance.OnTutorialStart -= StartRunning;
        GameManager.Instance.OnStageClear -= StopRunning;
        GameManager.Instance.OnStageStart -= StartRunning;
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

    public void StartRunning()
    {
        _isRunning = true;
    }
    public void StopRunning()
    {
        _isRunning = false;        
    }

}
