using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WorldValueBar : UI_World
{
    Transform _target;
    [SerializeField] UI_HUDStrategyData _strategyData;
    [SerializeField] Slider _slider;
    [SerializeField] Image _fillImage;
    void Start()
    {
        _fillImage.color = _strategyData.FillColor;
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }
    public override void Bind(Transform target)
    {
        _target = target;
    }
    public void UpdatePos()
    {
        Debug.Log(_target.position);
        transform.position = _target.position + Vector3.up;
    }
}
