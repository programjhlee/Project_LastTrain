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
        _closeButton.onClick.AddListener(base.Hide);

    }

    public void GoToTitleButtonClicked()
    {
        UI_Caution ui = UIManager.Instance.ShowPopupUIAt<UI_Caution>(Vector3.zero);
        ui.SetText("타이틀 화면으로 \n 돌아가시겠습니까?");
        ui.BindYesButton(() => { base.Hide(); _gameScene.RestartScene(); });
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
