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
        _announce.TextPanelRect.sizeDelta = new Vector2(1160, 240);
        _announce.TextRect.sizeDelta = new Vector2(925, 240);
    }

  
}
