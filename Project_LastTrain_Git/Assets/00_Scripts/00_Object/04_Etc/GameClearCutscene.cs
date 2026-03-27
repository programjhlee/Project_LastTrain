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

    public override CutsceneManager.CutsceneType CutsceneType => CutsceneManager.CutsceneType.GameClear;

    public override void CutsceneClear()
    {
        _gameClearImage.gameObject.SetActive(true);
    }

    public override IEnumerator CutsceneExecute()
    {
        UIManager.Instance.FadeIn();
        _gameClearImage.gameObject.SetActive(true);
        _gameClearText.GetComponent<RectTransform>().DOPunchScale(Vector3.one * 1.5f, 0.5f);
        yield return new WaitForSeconds(3f);
        UI_Caution uiCaution = UIManager.Instance.ShowPopupUIAt<UI_Caution>(Vector3.zero);
        uiCaution.SetText("DO YOU WANT RETURN TO TITLE?");
        uiCaution.BindYesButton(() => { CutsceneClear(); _gameScene.RestartScene(); });
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        UI_Caution uiCaution = UIManager.Instance.ShowPopupUIAt<UI_Caution>(Vector3.zero);
        uiCaution.SetText("DO YOU WANT RETURN TO TITLE?");
        uiCaution.BindYesButton(() => { CutsceneClear(); _gameScene.RestartScene(); });
    }
}
