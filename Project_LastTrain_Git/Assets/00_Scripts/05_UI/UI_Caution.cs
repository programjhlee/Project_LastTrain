using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UI_Caution : UI_Popup
{
    [SerializeField] TextMeshProUGUI _announceText;
    [SerializeField] Button _yesButton;
    [SerializeField] Button _noButton;
    public void OnEnable()
    {
        _yesButton.interactable = true;
        _noButton.interactable = true;
        _yesButton.onClick.AddListener(() => {_yesButton.interactable = false; base.Hide(); });
        _noButton.onClick.AddListener(() => { _noButton.interactable = false; base.Hide(); });
    }
    public void OnDisable()
    {
        _yesButton.onClick.RemoveAllListeners();
        _noButton.onClick.RemoveAllListeners();
    }

    public void BindYesButton(UnityAction action)
    {
        _yesButton.onClick.AddListener(action);
    }
    public void BindNoButton(UnityAction action)
    {
        _noButton.onClick.AddListener(action);
    }
    public void SetText(string text)
    {
        _announceText.text = text;
    }
}
