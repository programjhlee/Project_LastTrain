using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UI_StageAnnounce : UI_Base
{
    [SerializeField] TextMeshProUGUI _levelText;
    [SerializeField] TextMeshProUGUI _remainDistanceText;
    [SerializeField] TextMeshProUGUI _goText;
    [SerializeField] Vector3 _levelTextRectInitPos;
    [SerializeField] Vector3 _remainDistanceTextRectInitPos;
    [SerializeField] Vector3 _goTextRectInitPos;

    [SerializeField] AudioClip _showTextClip;
    [SerializeField] AudioClip _goTextClip;

    PlatformController _platformController;
    
    RectTransform _levelTextRect;
    RectTransform _remainDistanceTextRect;
    RectTransform _goTextRect;

    void Awake()
    {
        DOTween.Init();
        _platformController = GameObject.FindAnyObjectByType<PlatformController>().GetComponent<PlatformController>();
        _levelTextRect = _levelText.GetComponent<RectTransform>();
        _remainDistanceTextRect= _remainDistanceText.GetComponent<RectTransform>();
        _goTextRect = _goText.GetComponent<RectTransform>();
        _goText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public IEnumerator AnnounceProcess()
    {
        _levelTextRect.anchoredPosition = _levelTextRectInitPos;
        _remainDistanceTextRect.anchoredPosition = _remainDistanceTextRectInitPos;
        _goTextRect.anchoredPosition = _goTextRectInitPos;

        SetText(_levelText, $"LEVEL {LevelManager.Instance.Level}");
        SetText(_remainDistanceText, $"REMAIN {_platformController.PlatformDistance:F2}M");

        _levelTextRect.DOAnchorPosX(-300, 0.8f).SetEase(Ease.OutExpo);
        SoundManager.Instance.PlaySFX(_showTextClip);
        yield return new WaitForSeconds(1f);
        _remainDistanceTextRect.DOAnchorPosX(300, 0.8f).SetEase(Ease.OutExpo);
        SoundManager.Instance.PlaySFX(_showTextClip);
        yield return new WaitForSeconds(1f);
        _goTextRect.gameObject.SetActive(true);
        _goTextRect.DOShakePosition(1f,20,150);
        SoundManager.Instance.PlaySFX(_goTextClip);
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
        _goText.gameObject.SetActive(false);
    }

    public void SetText(TextMeshProUGUI _uiText, string text)
    {
        _uiText.text = text;
    }
    public override void Show()
    {
        base.Show();
        StartCoroutine(AnnounceProcess());
    }
}
