using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameClearCutscene : Cutscene, IPointerClickHandler
{
    [SerializeField] GameScene _gameScene;
    [SerializeField] Image _gameClearImage;
    [SerializeField] TextMeshProUGUI _gameClearText;
    UI_Caution _uiCaution;
    bool _isProcess;
    public override CutsceneManager.CutsceneType CutsceneType => CutsceneManager.CutsceneType.GameClear;

    public void Awake()
    {
        _gameScene = GameObject.FindAnyObjectByType<GameScene>();
        gameObject.SetActive(false);
    }
    public override void CutsceneClear()
    {
        _gameClearImage.gameObject.SetActive(false);
        SoundManager.Instance.StopBGM();
    }

    public override IEnumerator CutsceneExecute()
    {
        _isProcess = true;
        SoundManager.Instance.VolumeFadeIn();
        SoundManager.Instance.PlayBGM(SoundManager.BGMType.GameAllClear);
        UIManager.Instance.FadeIn();
        _gameClearImage.gameObject.SetActive(true);
        RectTransform gameClearTextRect = _gameClearText.GetComponent<RectTransform>();
        gameClearTextRect.DOPunchScale(Vector3.one * 1.1f, 0.5f).OnComplete(
            () => gameClearTextRect.DOAnchorPosY(50f, 1f).SetRelative(true).SetLoops(-1, LoopType.Yoyo)
            );
        yield return new WaitForSeconds(3f);
        _uiCaution = UIManager.Instance.ShowPopupUIAt<UI_Caution>(Vector3.zero);
        _uiCaution.SetText("DO YOU WANT RETURN TO TITLE?");
        _uiCaution.BindYesButton(() => { CutsceneClear(); _gameScene.RestartScene(); });
        _isProcess = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isProcess && _uiCaution == null)
        {
            _uiCaution = UIManager.Instance.ShowPopupUIAt<UI_Caution>(Vector3.zero);
            _uiCaution.SetText("DO YOU WANT RETURN TO TITLE?");
            _uiCaution.BindYesButton(() => { CutsceneClear(); _gameScene.RestartScene(); });
        }
    }
}
