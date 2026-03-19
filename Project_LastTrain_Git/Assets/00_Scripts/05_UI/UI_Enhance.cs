using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Enhance : UI_Base
{
    [SerializeField] Text _playerLevelText;
    [SerializeField] Text _trainHpText;
    [SerializeField] Button _enhanceButton;
    [SerializeField] Button _fixButton;

    RectTransform _enhanceButtonRect;
    RectTransform _fixButtonRect;

    void Awake()
    {
        _enhanceButtonRect = _enhanceButton.GetComponent<RectTransform>();
        _fixButtonRect = _fixButton.GetComponent<RectTransform>();
    }

    public void OnEnable()
    {
        EnhanceManager.Instance.OnPlayerEnhance += SetPlayerDataText;
        _enhanceButton.onClick.AddListener(Enhance);
        _fixButton.onClick.AddListener(EnhanceManager.Instance.FixTrain);
        
    }

    public void OnDisable()
    {
        EnhanceManager.Instance.OnPlayerEnhance -= SetPlayerDataText;
        _enhanceButton.onClick.RemoveListener(Enhance);
        _fixButton.onClick.RemoveListener(EnhanceManager.Instance.FixTrain);
    }

    public void SetPlayerDataText(PlayerData playerData)
    {
        Debug.Log(playerData.Level);
        ButtonShake(_enhanceButtonRect);
    }

    public void Enhance()
    {
        EnhanceManager.Instance.Enhance();
        ButtonShake(_enhanceButtonRect);
    }

    public void ButtonShake(RectTransform button)
    {
        button.DOShakePosition(0.3f,5f,20);
    }

}
