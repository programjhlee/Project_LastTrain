using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ControlGuide : UI_HUD
{
    UI_HUDControlGuideStrategyData _uiControlGuideData;
    [SerializeField] Text _uiGuideText;
    [SerializeField] Image _uiGuideImage; 
    public void BindData(UI_HUDControlGuideStrategyData uiData)
    {
        _uiControlGuideData = uiData;
        _uiGuideImage.sprite = _uiControlGuideData.ControlGuideKeyImage;
        _uiGuideText.text = _uiControlGuideData.ControlGuideText;
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

}
