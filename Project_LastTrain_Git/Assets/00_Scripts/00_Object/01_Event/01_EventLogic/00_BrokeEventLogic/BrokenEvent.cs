using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BrokenEvent : Event,ITrainDamageEvent
{
    float curTime = 0;
    float curFixAmount = 0;
    UIHUDController _evtHUDController;
    BrokenEventSound _brokenEventSound;
    public event Action<float> OnDamage;

    public void Awake()
    {
        _brokenEventSound = GetComponent<BrokenEventSound>();
        _evtHUDController = GetComponent<UIHUDController>();
        _brokenEventSound.OnAwake();
    }

    public override void Enter(EventData initEventData)
    {
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
        InvokeOnFix();
        ReleaseActionEvent();
        OnDamage = null;
        _evtHUDController.UIHUDListClear();
        _brokenEventSound.PlayExitSound();
        gameObject.SetActive(false);

    }
    public override void TakeFix(float fixPower)
    {
        curFixAmount -= fixPower;
        InvokeTakeFix(curFixAmount / EventData.FixAmount);
        if(curFixAmount <= 0)
        {
            Exit();
        }
    }
}
