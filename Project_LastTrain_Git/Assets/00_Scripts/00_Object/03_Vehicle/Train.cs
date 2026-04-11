using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Train : MonoBehaviour
{
    [SerializeField] GameObject _trainGameObject;
    
    bool _isRunning = false;
    float _maxHp;
    float _curHp;

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

    public event Action OnReset;
    public event Action<float> OnDamaged;
    public event Action<float> OnHpChanged;
    public event Action OnTrainDestroy;
    public void OnEnable()
    {
        GameManager.Instance.OnTutorialStart += StartRunning;
        GameManager.Instance.OnAllStageClear += StopRunning;
        GameManager.Instance.OnStageClear += StopRunning;
        GameManager.Instance.OnStageStart += StartRunning;
    }
    public void OnDisable()
    {
        GameManager.Instance.OnTutorialStart -= StartRunning;
        GameManager.Instance.OnAllStageClear -= StopRunning;
        GameManager.Instance.OnStageClear -= StopRunning;
        GameManager.Instance.OnStageStart -= StartRunning;
    }
    public void Start()
    {
        SoundManager.Instance.AddObjAudioSource(gameObject);
    }
    public void TakeDamage(float damage)
    {
        _curHp -= damage;
        OnHpChanged?.Invoke(_curHp/_maxHp);
        if (_curHp <= 0)
        {
            StopRunning();
            OnTrainDestroy?.Invoke();
            return;
        }
        OnDamaged?.Invoke(damage);
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
    public void ResetTrain()
    { 
        _trainGameObject.SetActive(true);
        StopRunning();
        _maxHp = 100;
        _curHp = _maxHp;
        OnHpChanged?.Invoke(_curHp / _maxHp);
        OnReset?.Invoke();
    }
}
