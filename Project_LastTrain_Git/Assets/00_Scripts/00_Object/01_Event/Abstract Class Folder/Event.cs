using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : MonoBehaviour, IFixable
{
    public EventData EventData { get; protected set; }
    public event Action OnFixed;
    public event Action<float> OnTakeFix;
    public abstract void Enter(EventData initEventData, float x = 0, float y = 0);
    public abstract void Execute();
    public abstract void Exit();

    public abstract void TakeFix(float fixPower);
    public void InvokeOnFix()
    {
        OnFixed?.Invoke();
    }
    public void InvokeTakeFix(float fixRatio)
    {
        OnTakeFix?.Invoke(fixRatio);
    }
    public void ReleaseActionEvent()
    {
        OnFixed = null;
        OnTakeFix = null;
    }
}
