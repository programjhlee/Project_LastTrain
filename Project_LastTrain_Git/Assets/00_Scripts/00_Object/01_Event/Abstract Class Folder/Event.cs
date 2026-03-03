using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : MonoBehaviour, IFixable
{
    public EventData eventData;
    public event Action OnFixed;
    public event Action<float> OnTakeFix;
    public virtual void Enter(EventData initEventData)
    {
        eventData = initEventData;
    }
    public abstract void Execute();
    public abstract void Exit();
    public abstract void TakeFix(float fixPower);
    protected void InvokeOnFix()
    {
        OnFixed?.Invoke();
    }
    protected void InvokeTakeFix(float fixRatio)
    {
        OnTakeFix?.Invoke(fixRatio);
    }

    protected void ReleaseActionEvent()
    {
        OnFixed = null;
        OnTakeFix = null;
    }
}
