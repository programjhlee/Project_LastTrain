using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI_HUD : UI_Base
{
    protected Transform _target;
    protected float _upDirScale;
    
    public virtual void Bind(Transform target,float upDirScale)
    {
        _target = target;
        _upDirScale = upDirScale;
    }

    public virtual void SetUpDirScale(float scale)
    {
        _upDirScale = scale;
    }

    public virtual void UpdatePos()
    {
        transform.position = _target.position + (Vector3.up * _upDirScale);
    }
}
