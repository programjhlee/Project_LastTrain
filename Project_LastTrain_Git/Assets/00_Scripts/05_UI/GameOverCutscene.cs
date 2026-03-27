using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverCutscene : Cutscene
{
    [SerializeField] Canvas _canvas;
    [SerializeField] GameScene _gameScene;
    //[SerializeField] Sprite _gameOverCutImage;
    [SerializeField] Text _gameOverText;

    public override CutsceneManager.CutsceneType CutsceneType => CutsceneManager.CutsceneType.GameOver;

    public void Awake()
    {
        _gameScene = GameObject.FindAnyObjectByType<GameScene>();
        gameObject.SetActive(false);
    }
    public override void CutsceneClear()
    {
        gameObject.SetActive(false);
    }
    public override IEnumerator CutsceneExecute()
    {
        gameObject.SetActive(true);
        UIManager.Instance.FadeIn();
        _gameOverText.DOColor(new Color(255, 255, 255, 255f), 1f);
        yield return new WaitForSeconds(2f);
        UI_Caution uiCaution = UIManager.Instance.ShowPopupUIAt<UI_Caution>(Vector3.zero);
        uiCaution.SetText("DO YOU WANT RETURN TO TITLE?");
        uiCaution.BindYesButton(() => { CutsceneClear(); _gameScene.RestartScene(); });
    }

}
