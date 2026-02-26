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

    void Awake()
    {
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        _uiRect = GetComponent<RectTransform>();
    }

    public void SetStrategyData(UI_HUDValueBarStrategyData strategyData)
    {
        _strategyData = strategyData;
        _fillImage.color = _strategyData.FillColor;
        _uiRect.sizeDelta = _strategyData.UIHUDSize;

    }

    public void SetValue(float ratio)
    {
        _slider.value = ratio;
    }
}
