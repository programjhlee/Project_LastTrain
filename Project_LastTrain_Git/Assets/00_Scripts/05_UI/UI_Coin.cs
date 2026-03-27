using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using DG.Tweening.Core;
public class UI_Coin : UI_Base
{
    [SerializeField] TextMeshProUGUI coinText;
    RectTransform _uiRect;
    public void Awake()
    {
        _uiRect = GetComponent<RectTransform>();
        LootManager.Instance.OnItemCountChanged += SetCoinText;
    }
    public void OnEnable()
    {
        GameManager.Instance.OnGameOver += ()=> gameObject.SetActive(false);
    }
    public void SetCoinText(int coin)
    {
        _uiRect.transform.DOKill();
        coinText.text = $" X {coin:D2}";
        _uiRect.DOAnchorPosY(10f, 0.02f).SetRelative().SetLoops(2, LoopType.Yoyo);
        _uiRect.DOShakeScale(0.02f,0.2f);
    }

}
