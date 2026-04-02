using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AnnounceStrategyQuest", menuName = "Create AnnounceStrategy/AnnounceQuest")]
public class AnnounceStrategyQuest :ScriptableObject, IAnnounceStrategy
{
    [SerializeField] UI_Announce _announce;

    public void SetUI()
    {
        _announce.CheckBox.gameObject.SetActive(true);
        _announce.QuestImage.gameObject.SetActive(true);
        _announce.TextPanelRect.sizeDelta = new Vector2(760, 240);
        _announce.TextPanelRect.anchoredPosition = new Vector2(120, 0);
        _announce.TextRect.sizeDelta = new Vector2(520, 240);
        _announce.TextRect.anchoredPosition = new Vector2(120, 0);
        _announce.SetTextColor(Color.white);
    }

  
}
