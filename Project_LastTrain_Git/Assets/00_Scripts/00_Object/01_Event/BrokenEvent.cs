using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BrokenEvent : Event,ITrainDamageEvent
{
    float curTime = 0;
    float curFixAmount = 0;

    public event Action<float> OnDamage;
    public override void Enter(EventData initEventData)
    {
        base.Enter(initEventData);
        curFixAmount = eventData.fixAmount;
        Debug.Log(eventData.fixAmount);
    }
    public override void Execute()
    {
        curTime += Time.deltaTime;
        if(curTime > eventData.cyclePerTime)
        {
            curTime = 0;
            OnDamage?.Invoke(eventData.damageToTrain);
        }
    }
    public override void Exit()
    {
        OnDamage = null;
        Destroy(gameObject);
    }
    public override void TakeFix(float fixPower)
    {
        curFixAmount -= fixPower;
        if(curFixAmount <= 0)
        {
            Exit();
        }
    }
}
