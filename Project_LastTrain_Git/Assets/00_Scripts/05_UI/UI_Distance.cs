using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Distance : UI_Base
{
    [SerializeField] PlatformController _platformController;
    [SerializeField] TextMeshProUGUI _ui_distanceText;
    [SerializeField] RectTransform _uiTextRect;
    Vector3 _textRectOriginPos;
    public void Awake()
    {
        _platformController = GameObject.FindAnyObjectByType<PlatformController>();
        _textRectOriginPos = _uiTextRect.anchoredPosition;
    }
    public void OnEnable()
    {
        _platformController.OnDistanceChanged += SetDistanceText;
        _platformController.OnPlatformedStop += Hide;
        _platformController.OnReset += Hide;
    }
    public void OnDisable()
    {
        _platformController.OnDistanceChanged -= SetDistanceText;
        _platformController.OnPlatformedStop -= Hide;
        _platformController.OnReset -= Hide;
        _uiTextRect.anchoredPosition = _textRectOriginPos;
        Debug.Log(_uiTextRect.anchoredPosition);
    }

    public void SetDistanceText(float distance)
    { 
        _ui_distanceText.text = $"{distance:F2}M LEFT!!";
        DoMoveShake();
    }

    public void DoMoveShake()
    {
        _uiTextRect.DOShakePosition(0.2f,1,50).Loops();
    }
}
