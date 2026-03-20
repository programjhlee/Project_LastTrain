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
    }

    public void OnEnable()
    {
        EnhanceManager.Instance.OnPlayerEnhance += SetPlayerDataText;
        EnhanceManager.Instance.OnNotEnoughMoney += NotEnoughMoney;
        EnhanceManager.Instance.OnFixSucess += FixSuccess;
        EnhanceManager.Instance.OnPlayerEnhanceSucess += EnhanceSuccess;
        EnhanceManager.Instance.OnTrainHpFull += TrainHpFull;
        _enhanceButton.onClick.AddListener(Enhance);
        _fixButton.onClick.AddListener(Fix);
        
    }

    public void OnDisable()
    {
        EnhanceManager.Instance.OnPlayerEnhance -= SetPlayerDataText;
        EnhanceManager.Instance.OnNotEnoughMoney -= NotEnoughMoney;
        EnhanceManager.Instance.OnFixSucess -= FixSuccess;
        EnhanceManager.Instance.OnPlayerEnhanceSucess -= EnhanceSuccess;
        EnhanceManager.Instance.OnTrainHpFull -= TrainHpFull;
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

    public void NotEnoughMoney()
    {
        SetAnnounceText("돈이 부족합니다...");
        SoundManager.Instance.PlaySFX(_failSound);
    }
    public void TrainHpFull()
    {
        SetAnnounceText("열차의 체력이 가득 찼습니다...");
        SoundManager.Instance.PlaySFX(_failSound);
    }
    public void FixSuccess()
    {
        SetAnnounceText("수리 성공!");
        SoundManager.Instance.PlaySFX(_fixSound);
    }
    public void EnhanceSuccess()
    {
        SetAnnounceText("플레이어 강화 성공!");
        SoundManager.Instance.PlaySFX(_enhanceSound);
    }


    public void SetAnnounceText(string text)
    {
        _announceTextRect.DOKill();
        _announceTextRect.localPosition = Vector3.zero;
        _announceText.gameObject.SetActive(true);
        _announceText.text = text;
        _announceTextRect.DOLocalMoveY(50f, 0.5f).SetEase(Ease.OutBack).OnComplete(() => _announceText.gameObject.SetActive(false));
    }
}
