using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : MonoBehaviour, IFixable
{
    public EventData eventData;
    public event Action OnFixed;
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
    protected void ReleaseOnFix()
    {
        OnFixed = null;
    }
}
