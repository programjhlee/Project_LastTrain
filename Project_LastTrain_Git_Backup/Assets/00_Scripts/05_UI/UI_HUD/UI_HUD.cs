using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI_HUD : UI_Base
{ 
    protected Transform _target;
    protected float _upDirScale;
    protected RectTransform _canvasRect;
    protected Camera cam;
    protected RectTransform _rect;

    public virtual void Bind(Canvas canvas, Transform target,float upDirScale)
    {
        transform.SetParent(canvas.transform);
        _canvasRect = canvas.GetComponent<RectTransform>();
        cam = Camera.main;
        _target = target;
        _rect = GetComponent<RectTransform>();
        _upDirScale = upDirScale;
    }

    public virtual void SetUpDirScale(float scale)
    {
        _upDirScale = scale;
    }

    public virtual void UpdatePos()
    {
        Vector2 targetPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, cam.WorldToScreenPoint(_target.position + Vector3.up * _upDirScale), cam, out targetPos);
        _rect.localPosition = targetPos;
    }
}
