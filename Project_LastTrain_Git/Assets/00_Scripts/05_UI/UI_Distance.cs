using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Distance : UI_Base
{
    [SerializeField] PlatformController _platformController;
    [SerializeField] TextMeshProUGUI ui_distanceText;

    public void Awake()
    {
        _platformController = GameObject.FindAnyObjectByType<PlatformController>();
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
    }

    public void SetDistanceText(float distance)
    { 
        ui_distanceText.text = $"{distance:F2}M LEFT!!";
        DoMoveShake();
    }

    public void DoMoveShake()
    {
        transform.DOShakePosition(0.2f,1,50).Loops();
    }
}
