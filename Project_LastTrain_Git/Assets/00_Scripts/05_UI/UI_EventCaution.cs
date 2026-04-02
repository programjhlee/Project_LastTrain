using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class UI_EventCaution : UI_Base
{
    bool _isActive = false;
    Sequence _turnOnSequence;
    Sequence _turnOffSequence;
    [SerializeField] TextMeshProUGUI ui_eventCntText;
    [SerializeField] RectTransform _uiRect;
    [SerializeField] AudioClip _showSound;
    [SerializeField] AudioClip _hideSound;
    public void SetEventCount(int cnt)
    {
        ui_eventCntText.text = cnt.ToString();
    }

    public override void Show()
    {
        if (_isActive == false)
        {
            _turnOnSequence = DOTween.Sequence();
            _uiRect.localScale = Vector3.zero;
            base.Show();
            _turnOnSequence.Append(_uiRect.DOScaleX(1f, 0.2f).SetEase(Ease.OutExpo));
            _turnOnSequence.Append(_uiRect.DOScaleY(1f, 0.2f).SetEase(Ease.OutExpo));
            _isActive = true;
            SoundManager.Instance.PlaySFX(_showSound);
        }
    }
    public override void Hide()
    {
        if (_isActive == true)
        {
            _isActive = false;
            _turnOnSequence.Kill();
            _turnOffSequence = DOTween.Sequence();
            _turnOffSequence.Append(_uiRect.DOScaleY(0f, 0.2f).SetEase(Ease.OutExpo));
            _turnOffSequence.Append(_uiRect.DOScaleX(0f, 0.2f).SetEase(Ease.OutExpo));
            _turnOffSequence.Play().OnComplete(() => base.Hide());
            SoundManager.Instance.PlaySFX(_hideSound);
        }
    }
}
