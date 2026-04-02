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
        _returnTitleButton.interactable = true;
        _closeButton.interactable = true;
        _optionButton.interactable = true;
        _optionButton.onClick.AddListener(_gameScene.OptionButtonClicked);
        _closeButton.onClick.AddListener(base.Hide);

    }

    public void GoToTitleButtonClicked()
    {
        _returnTitleButton.interactable = false;
        _closeButton.interactable = false;
        _optionButton.interactable = false;
        UI_Caution ui = UIManager.Instance.ShowPopupUIAt<UI_Caution>(Vector3.zero);
        ui.SetText("ARE YOU SURE TO BACK TO THE TITLE?");
        ui.BindYesButton(() => { base.Hide(); _gameScene.RestartScene(); });
        ui.BindNoButton(() => 
        { 
            base.Hide(); 
            _returnTitleButton.interactable = false;
            _closeButton.interactable = false;
            _optionButton.interactable = false;
        
        });
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
