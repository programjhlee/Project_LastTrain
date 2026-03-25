using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UI_Popup : UI_Base
{
    [SerializeField] RectTransform _uiRect;
    [SerializeField] AudioClip _showSound;
    [SerializeField] AudioClip _hideSound;
    public override void Show()
    {
        _uiRect.localScale = Vector3.zero;
        base.Show();
        _uiRect.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        SoundManager.Instance.PlaySFX(_showSound);
    }
    public override void Hide()
    {
        _uiRect.DOScale(0, 0.3f).SetEase(Ease.InBack).OnComplete(()=>Destroy(gameObject));
        SoundManager.Instance.PlaySFX(_hideSound);
    }
}
