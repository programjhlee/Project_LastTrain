using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UI_Announce : UI_Base
{
    [SerializeField] TextMeshProUGUI _questText;
    [SerializeField] GameObject _checkBox;
    [SerializeField] Image _checkImage;

    public void Init()
    {
        _checkImage.gameObject.SetActive(false);
    }

    public void ShowCheckBox()
    {
        _checkBox.SetActive(true);
    }
    public void QuestClear()
    {
        _checkImage.gameObject.SetActive(true);
    }

    public void SetQuestText(string text)
    {
        _questText.text = text;
    }
}
