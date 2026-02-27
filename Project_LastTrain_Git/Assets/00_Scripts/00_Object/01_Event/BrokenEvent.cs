using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BrokenEvent : Event,ITrainDamageEvent
{
    float curTime = 0;
    float curFixAmount = 0;
    UIHUDStack _evtHUDController;

    public event Action<float> OnDamage;
    public override void Enter(EventData initEventData)
    {
        base.Enter(initEventData);
        _evtHUDController = GetComponent<UIHUDStack>();
        curFixAmount = eventData.fixAmount;
    }
    public override void Execute()
    {
        curTime += Time.deltaTime;
        _evtHUDController.UpdateUIHUDPos();
        if(curTime > eventData.cyclePerTime)
        {
            curTime = 0;
            OnDamage?.Invoke(eventData.damageToTrain);
        }
    }
    public override void Exit()
    {
        InvokeOnFix();
        ReleaseActionEvent();
        OnDamage = null;
        Destroy(gameObject);
    }
    public override void TakeFix(float fixPower)
    {
        curFixAmount -= fixPower;
        InvokeTakeFix(curFixAmount / eventData.fixAmount);
        if(curFixAmount <= 0)
        {
            Exit();
        }
    }
}
