using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "AnnounceStrategyAlert", menuName = "Create AnnounceStrategy/AnnounceAlert")]
public class AnnounceStrategyAlert : ScriptableObject, IAnnounceStrategy
{
    [SerializeField] UI_Announce _announce;
    public void SetUI()
    {
        _announce.CheckBox.gameObject.SetActive(false);
        _announce.QuestImage.gameObject.SetActive(false);
        _announce.TextPanelRect.sizeDelta = new Vector2(1000, 240);
        _announce.TextRect.sizeDelta = new Vector2(1000, 240);
        _announce.TextPanelRect.anchoredPosition = Vector2.zero;
        _announce.TextRect.anchoredPosition = Vector2.zero;
        _announce.SetTextColor(Color.red);
    }
}
