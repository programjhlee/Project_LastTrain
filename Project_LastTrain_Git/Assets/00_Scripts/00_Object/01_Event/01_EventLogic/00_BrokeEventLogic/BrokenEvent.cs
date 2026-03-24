using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BrokenEvent : Event,ITrainDamageEvent
{
    ParticleSystem[] _effects;
    [SerializeField] GameObject _fixEffect;
    Collider _col;
    float curTime = 0;
    float curFixAmount = 0;
    UIHUDController _evtHUDController;
    BrokenEventSound _brokenEventSound;
    public event Action<float> OnDamage;

    public void Awake()
    {
        _col = GetComponent<Collider>();
        _effects = GetComponentsInChildren<ParticleSystem>();
        _brokenEventSound = GetComponent<BrokenEventSound>();
        _evtHUDController = GetComponent<UIHUDController>();
        _brokenEventSound.OnAwake();
    }

    public override void Enter(EventData initEventData)
    {
        for(int i = 0; i < _effects.Length; i++)
        {
            var main = _effects[i].main;
            main.loop = true;
        }
        _col.enabled = true;
        curTime = 0;
        EventData = initEventData;
        _evtHUDController.Init();
        curFixAmount = EventData.FixAmount;
        _brokenEventSound.PlayEnterSound();
    }
    public override void Execute()
    {
        curTime += Time.deltaTime;
        _evtHUDController.UpdateUIHUDPos();
        if(curTime > EventData.CyclePerTime)
        {
            curTime = 0;
            OnDamage?.Invoke(EventData.DamageToTrain);
        }
    }
    public override void Exit()
    {
        StartCoroutine(ExitProcess());

    }
    IEnumerator ExitProcess()
    {
        _col.enabled = false;
        for (int i = 0; i < _effects.Length; i++)
        {
            var main = _effects[i].main;
            main.loop = false;
        }
        ReleaseActionEvent();
        OnDamage = null;
        _evtHUDController.UIHUDListClear();
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
    public override void TakeFix(float fixPower)
    {
        curFixAmount -= fixPower;
        InvokeTakeFix(curFixAmount / EventData.FixAmount);
        Instantiate(_fixEffect,transform.position,Quaternion.identity);
        if(curFixAmount <= 0)
        {
            InvokeOnFix();
            _brokenEventSound.PlayExitSound();
            Exit();
        }
    }
}
