using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_HUDControlGuide : UI_HUD
{
    UI_HUDControlGuideStrategyData _uiControlGuideData;
    [SerializeField] Text _uiGuideText;
    [SerializeField] Image _uiGuideImage;
    string _uiGuideStr;
    public void BindData(UI_HUDControlGuideStrategyData uiData)
    {
        _uiControlGuideData = uiData;
        _uiGuideImage.sprite = uiData.ControlGuideKeyImage;
        _uiGuideStr = uiData.ControlGuideText;
        transform.localScale = new Vector2(1, 0);
    }

    public override void Show()
    {
        base.Show();
        _rect.DOScaleY(1, 0.2f).SetEase(Ease.OutCirc);

    }
    public override void Hide()
    {
        _rect.DOScaleY(0, 0.2f).SetEase(Ease.OutCirc).OnComplete(()=> { base.Hide(); });
    }


    public override void UpdatePos()
    {
        Vector2 targetPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, cam.WorldToScreenPoint(_target.position + Vector3.up * _upDirScale), cam, out targetPos);
        float hover = (Mathf.Sin(Time.time * 3f) + 1) * 15f;
        _rect.localPosition = new Vector2(targetPos.x,targetPos.y + hover);
        SetText(_uiGuideStr);

    }
    public void SetText(string text)
    {
        _uiGuideText.text = text;
    }
}
