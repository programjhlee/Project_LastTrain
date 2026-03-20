using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UI_Popup : UI_Base
{
    [SerializeField] RectTransform _uiRect;

    public override void Show()
    {
        base.Show();
        _uiRect.DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }
    public void ClickedYesbutton()
    {
        Application.Quit();
    }
    public override void Hide()
    {
        _uiRect.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(()=>base.Hide());
    }
}
