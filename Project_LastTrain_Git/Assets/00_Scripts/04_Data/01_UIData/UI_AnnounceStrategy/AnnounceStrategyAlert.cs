using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnnounceStrategyAlert", menuName = "Create AnnounceStrategy/AnnounceAlert")]
public class AnnounceStrategyAlert : ScriptableObject, IAnnounceStrategy
{
    [SerializeField] UI_Announce _announce;
    [SerializeField] string _alertText;
    public void SetUI()
    {
        _announce.CheckBox.gameObject.SetActive(false);
        _announce.QuestImage.gameObject.SetActive(false);
        
    }
}
