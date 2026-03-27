using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOverCutscene : Cutscene, IPointerClickHandler
{
    [SerializeField] Canvas _canvas;
    [SerializeField] GameScene _gameScene;
    //[SerializeField] Sprite _gameOverCutImage;
    [SerializeField] Text _gameOverText;
    UI_Caution _uiCaution;
    bool _isProcess;
    public override CutsceneManager.CutsceneType CutsceneType => CutsceneManager.CutsceneType.GameOver;

    public void Awake()
    {
        _gameScene = GameObject.FindAnyObjectByType<GameScene>();
        gameObject.SetActive(false);
    }
    public override void CutsceneClear()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
    public override IEnumerator CutsceneExecute()
    {
        _isProcess = true;
        gameObject.SetActive(true);
        UIManager.Instance.FadeIn();
        _gameOverText.DOColor(new Color(255, 255, 255, 255f), 1f);
        yield return new WaitForSeconds(2f);
        _isProcess = false;
        ShowReturnToTitleCaution();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_uiCaution != null || _isProcess)
        {
            return;
        }
        ShowReturnToTitleCaution();
    }

    public void ShowReturnToTitleCaution()
    {
        _uiCaution = UIManager.Instance.ShowPopupUIAt<UI_Caution>(Vector3.zero);
        _uiCaution.SetText("DO YOU WANT RETURN TO TITLE?");
        _uiCaution.BindYesButton(() => { StopCoroutine(CutsceneExecute()); _gameScene.RestartScene(); });
    }
}
