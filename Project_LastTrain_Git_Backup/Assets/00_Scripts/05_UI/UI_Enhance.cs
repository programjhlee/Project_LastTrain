using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UI_Enhance : UI_Base
{
    [SerializeField] Text _playerLevelText;
    [SerializeField] Text _trainHpText;
    [SerializeField] TextMeshProUGUI _levelUpCostText;
    [SerializeField] TextMeshProUGUI _fixCostText;
    [SerializeField] TextMeshProUGUI _announceText;

    [SerializeField] Button _enhanceButton;
    [SerializeField] Button _fixButton;


    [SerializeField] AudioClip _fixSound;
    [SerializeField] AudioClip _enhanceSound;
    [SerializeField] AudioClip _failSound;

    RectTransform _enhanceButtonRect;
    RectTransform _fixButtonRect;
    RectTransform _announceTextRect;
    void Awake()
    {
        _enhanceButtonRect = _enhanceButton.GetComponent<RectTransform>();
        _fixButtonRect = _fixButton.GetComponent<RectTransform>();
        _announceTextRect = _announceText.GetComponent<RectTransform>();
        _announceText.gameObject.SetActive(false);
        _levelUpCostText.text = $"COST {EnhanceManager.Instance.EnhancePrice}";
        _fixCostText.text = $"COST {EnhanceManager.Instance.FixPrice}";
    }

    public void OnEnable()
    {
        EnhanceManager.Instance.OnPlayerEnhance += SetPlayerDataText;
        EnhanceManager.Instance.OnNotEnoughMoney += AnnounceNotEnoughMoney;
        EnhanceManager.Instance.OnFixSucess += AnnounceFixSuccess;
        EnhanceManager.Instance.OnPlayerEnhanceSucess += AnnounceEnhanceSuccess;
        EnhanceManager.Instance.OnTrainHpFull += AnnounceTrainHpFull;
        _enhanceButton.onClick.AddListener(Enhance);
        _fixButton.onClick.AddListener(Fix);
        
    }

    public void OnDisable()
    {
        EnhanceManager.Instance.OnPlayerEnhance -= SetPlayerDataText;
        EnhanceManager.Instance.OnNotEnoughMoney -= AnnounceNotEnoughMoney;
        EnhanceManager.Instance.OnFixSucess -= AnnounceFixSuccess;
        EnhanceManager.Instance.OnPlayerEnhanceSucess -= AnnounceEnhanceSuccess;
        EnhanceManager.Instance.OnTrainHpFull -= AnnounceTrainHpFull;
        _enhanceButton.onClick.RemoveListener(Enhance);
        _fixButton.onClick.RemoveListener(Fix);
    }

    public void SetPlayerDataText(PlayerData playerData)
    {

    }

    public void Enhance()
    {
        EnhanceManager.Instance.Enhance();
        ButtonShake(_enhanceButtonRect);
    }
    public void Fix()
    {
        EnhanceManager.Instance.FixTrain();
        ButtonShake(_fixButtonRect);
    }


    public void ButtonShake(RectTransform button)
    {
        button.DOShakePosition(0.3f,5f,20);
    }

    public void AnnounceNotEnoughMoney()
    {
        SetAnnounceText("NOT ENOUGH MONEY..", Color.red);
        SoundManager.Instance.PlaySFX(_failSound);
    }
    public void AnnounceTrainHpFull()
    {
        SetAnnounceText("TRAIN HP IS FULL!!", Color.red);
        SoundManager.Instance.PlaySFX(_failSound);
    }
    public void AnnounceFixSuccess()
    {
        SetAnnounceText("FIX COMPLETE!");
        SoundManager.Instance.PlaySFX(_fixSound);
    }
    public void AnnounceEnhanceSuccess()
    {
        SetAnnounceText("LEVEL UP!");
        SoundManager.Instance.PlaySFX(_enhanceSound);
    }


    public void SetAnnounceText(string text,Color color = default)
    {
        if (color == default) color = Color.white;

        _announceText.color = color;
        _announceTextRect.DOKill();
        _announceTextRect.localPosition = Vector3.zero;
        _announceText.gameObject.SetActive(true);
        _announceText.text = text;
        _announceTextRect.DOLocalMoveY(50f, 0.5f).SetEase(Ease.OutBack).OnComplete(() => _announceText.gameObject.SetActive(false));
    }
}
