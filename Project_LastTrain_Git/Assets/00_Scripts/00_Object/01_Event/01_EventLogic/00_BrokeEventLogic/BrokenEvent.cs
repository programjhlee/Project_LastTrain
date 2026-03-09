using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BrokenEvent : Event,ITrainDamageEvent
{
    float curTime = 0;
    float curFixAmount = 0;
    UIHUDController _evtHUDController;

    public event Action<float> OnDamage;
    public override void Enter(EventData initEventData)
    {
        EventData = initEventData;
        _evtHUDController = GetComponent<UIHUDController>();
        _evtHUDController.Init();
        curFixAmount = EventData.FixAmount;
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
