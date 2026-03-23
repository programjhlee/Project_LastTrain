using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
public class UI_Announce : UI_Popup
{
    [SerializeField] List<IAnnounceStrategy> _strategyList;
    [SerializeField] TextMeshProUGUI _announceText;
    [SerializeField] GameObject _checkBox;
    [SerializeField] Image _questImage;
    [SerializeField] Image _checkImage;
    [SerializeField] RectTransform _textPanelRect;
    [SerializeField] RectTransform _textRect;
    RectTransform _checkImageRect;

    public GameObject CheckBox => _checkBox;
    public TextMeshProUGUI AnnounceText => _announceText;
    public Image QuestImage => _questImage;
    public RectTransform TextRect => _textRect;
    public RectTransform TextPanelRect => _textPanelRect;


    public void Awake()
    {
        _checkImageRect = _checkImage.GetComponent<RectTransform>();
    }
    public void Init()
    {
        _checkImage.gameObject.SetActive(false);
    }

    public void SetUIStrategy(IAnnounceStrategy strategy)
    {
        strategy.SetUI();
    }

    public void SetQuestSprite(Sprite sprite)
    {
        _questImage.sprite = sprite;
    }
    public void ShowCheckBox()
    {
        _checkBox.SetActive(true);
    }
    public void QuestClear()
    {
        _checkImageRect.localScale = Vector3.zero;
        _checkImage.gameObject.SetActive(true);
        Sequence clearSequence = DOTween.Sequence();
        clearSequence.Append(_checkImageRect.DOScaleY(1.2f, 0.1f).SetEase(Ease.OutCirc));
        clearSequence.Append(_checkImageRect.DOScaleX(1.2f, 0.1f).SetEase(Ease.OutCirc));
        clearSequence.Append(_checkImageRect.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutCirc).SetLoops(2,LoopType.Yoyo));
        clearSequence.Play();
        _checkImage.gameObject.SetActive(true);
    }

    public void SetAnnounceText(string text)
    {
        _announceText.text = text;
    }
    public void SetImage(Sprite _image = null)
    {
        _checkImage.sprite = _image;
    }
}
