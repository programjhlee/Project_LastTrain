using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class UI_Menu : UI_Popup
{
    GameScene _gameScene;
    [SerializeField] Button _optionButton;
    [SerializeField] Button _returnTitleButton;
    [SerializeField] Button _closeButton;

    public event Action OnMenuClosed;
    public void Awake()
    {
        _gameScene = GameObject.FindAnyObjectByType<GameScene>();
    }
    public void OnEnable()
    {
        _optionButton.onClick.AddListener(_gameScene.OptionButtonClicked);
        _returnTitleButton.onClick.AddListener(_gameScene.ReturnToTitle);
        _closeButton.onClick.AddListener(base.Hide);

    }
    public void OnDestroy()
    {
        _optionButton.onClick.RemoveListener(_gameScene.OptionButtonClicked);
        _returnTitleButton.onClick.RemoveListener(_gameScene.ReturnToTitle);
        _closeButton.onClick.RemoveListener(base.Hide);
        OnMenuClosed?.Invoke();
        OnMenuClosed = null;
    }
}
