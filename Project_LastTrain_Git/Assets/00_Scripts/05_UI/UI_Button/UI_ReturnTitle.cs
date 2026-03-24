using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ReturnTitle : UI_Base
{
    [SerializeField] GameScene _gameScene;
    public void ReturnToTitle()
    {
        GameManager.Instance.GamePaused();
        UI_Caution _uiCaution = UIManager.Instance.ShowPopupUIAt<UI_Caution>(Vector3.zero);
        _uiCaution.SetText("타이틀 화면으로 돌아가시겠습니까?");
        _uiCaution.BindYesButton(() => {  _gameScene.RestartScene(); });
        _uiCaution.BindNoButton(() => { Debug.Log("게임 재시작"); GameManager.Instance.GameResume(); });
    }
}
