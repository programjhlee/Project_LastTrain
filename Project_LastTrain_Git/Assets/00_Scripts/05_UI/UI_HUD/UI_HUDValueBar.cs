using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HUDValueBar : UI_HUD
{
    [SerializeField] Slider _slider;
    [SerializeField] Image _fillImage;
    RectTransform _uiRect;
    UI_HUDValueBarStrategyData _strategyData;
    public void Init(UI_HUDValueBarStrategyData strategyData)
    { 
        _uiRect = GetComponent<RectTransform>();
        _strategyData = strategyData;
        _fillImage.color = _strategyData.FillColor;
        _uiRect.sizeDelta = _strategyData.UIHUDSize;
        transform.localScale = new Vector3(1, 1, 1);
        _slider.value = 1;
    }

    public void SetValue(float ratio)
    {
        _slider.value = ratio;
    }
}
