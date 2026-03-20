using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class GameScene : MonoBehaviour
{
    [SerializeField] Train _train;
    [SerializeField] PlatformController _platformController;
    [SerializeField] EnemySpawner _enemySpawner;
    [SerializeField] UI_Title _uiTitle;
    [SerializeField] TutorialSystem _tutorialSystem;
    [SerializeField] Button _startButton;
    [SerializeField] Button _optionButton;
    [SerializeField] Button _exitButton;



    public void OnEnable()
    {
        _platformController.OnPlatformRunning += () => { UIManager.Instance.ShowUIAt<UI_Distance>(new Vector2(0, 350f)); };
        _platformController.OnReset += () => { UIManager.Instance.HideUI<UI_Distance>(); };
        _platformController.OnDistanceZero += () => { UIManager.Instance.HideUI<UI_Distance>(); };
    }
    public void Start()
    {
        SceneStart();
    }

    public void SceneStart()
    {
        UIManager.Instance.FadeIn();
        LevelManager.Instance.ResetLevel();
        CameraManager.Instance.SetStartCamPrioirty();
        _uiTitle.Show();
        _startButton.gameObject.SetActive(true);
        _exitButton.gameObject.SetActive(true);
        _optionButton.gameObject.SetActive(true);
        _train.ResetTrain();
        _platformController.ResetPlatform();
        _enemySpawner.SetEnemiesData();
    }
    public void SceneClear()
    {
        GameManager.Instance.GamePaused();
        _train.ResetTrain();
        _enemySpawner.AllEnemyClear();
    }

    public void RestartScene()
    {
        StartCoroutine(RestartProcess());
    }

    public void StartButtonClicked()
    {
        _uiTitle.Hide();
        _startButton.gameObject.SetActive(false);
        _exitButton.gameObject.SetActive(false);
        _optionButton.gameObject.SetActive(false);
        GameManager.Instance.TutorialStart();
    }
    public void OptionButtonClicked()
    {
        UIManager.Instance.ShowUI<UI_Option>();
    }
    public void ExitButtonClicked()
    {
        UI_Caution ui_popup = UIManager.Instance.ShowUI<UI_Caution>();
        ui_popup.BindYesButton(()=> Application.Quit());
        ui_popup.SetText("정말로 종료하시겠습니까?");
    }

    IEnumerator RestartProcess()
    {
        UIManager.Instance.FadeOut();
        yield return new WaitForSeconds(0.5f);
        SceneClear();
        SceneStart();
        yield return new WaitForSeconds(1f);
        UIManager.Instance.FadeIn();
    }
}
