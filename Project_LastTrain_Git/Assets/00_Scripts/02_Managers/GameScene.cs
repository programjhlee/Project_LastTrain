using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class GameScene : MonoBehaviour
{
    [SerializeField] AudioClip _titleSound;
    [SerializeField] Train _train;
    [SerializeField] PlayerAction _player;
    [SerializeField] PlatformController _platformController;
    [SerializeField] EnemySpawner _enemySpawner;
    [SerializeField] UI_Title _uiTitle;
    [SerializeField] TutorialSystem _tutorialSystem;
    [SerializeField] Button _startButton;
    [SerializeField] Button _optionButton;
    [SerializeField] Button _exitButton;
    [SerializeField] Button _menuButton;



    public void OnEnable()
    {
        _platformController.OnPlatformRunning += () => { UIManager.Instance.ShowUIAt<UI_Distance>(new Vector2(0, 350f)); };
        _platformController.OnReset += () => { UIManager.Instance.HideUI<UI_Distance>(); };
        _platformController.OnDistanceZero += () => { UIManager.Instance.HideUI<UI_Distance>(); };
        _train.OnTrainDestroy += _player.OnTrainDestroy;
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
        CameraManager.Instance.BlendModeInit();
        SoundManager.Instance.PlayBGM(_titleSound);
        CutsceneManager.Instance.CurrentSceneClear();
        LootManager.Instance.ResetItemCount();
        _uiTitle.Show();
        _startButton.gameObject.SetActive(true);
        _exitButton.gameObject.SetActive(true);
        _player.gameObject.SetActive(true);
        _optionButton.gameObject.SetActive(true);
        _menuButton.gameObject.SetActive(false);
        _train.ResetTrain();
        _platformController.ResetPlatform();
        _enemySpawner.SetEnemiesData();
    }
    public void SceneClear()
    {
        GameManager.Instance.ResetGameManager();
        SoundManager.Instance.StopBGM();
        UIManager.Instance.HideUI<UI_Enhance>();
        UIManager.Instance.HideUI<UI_Coin>();
        UIManager.Instance.HideUI<UI_TrainHP>();
        UIManager.Instance.HideUI<UI_StageAnnounce>();
        _train.ResetTrain();
        _enemySpawner.AllEnemyClear();
        _player.ResetPlayerData();
        _menuButton.gameObject.SetActive(false);
        _tutorialSystem.ResetTutorialSystem();
    }

    public void RestartScene()
    {
        StartCoroutine(RestartProcess());
    }

    public void StartButtonClicked()
    {
        _uiTitle.Hide();
        SoundManager.Instance.StopBGM();
        _startButton.gameObject.SetActive(false);
        _exitButton.gameObject.SetActive(false);
        _optionButton.gameObject.SetActive(false);
        GameManager.Instance.TutorialStart();
    }
    public void OptionButtonClicked()
    {
        UIManager.Instance.ShowPopupUIAt<UI_Option>(Vector3.zero);
    }
    public void MenuButtonClicked()
    {
        UIManager.Instance.ShowPopupUIAt<UI_Menu>(Vector3.zero);
        GameManager.Instance.GamePaused();
    }
    public void ExitButtonClicked()
    {
        UI_Caution ui_popup = UIManager.Instance.ShowPopupUIAt<UI_Caution>(Vector3.zero);
        ui_popup.BindYesButton(()=> Application.Quit());
        ui_popup.SetText("ARE YOU SURE TO QUIT?");
    }

    IEnumerator RestartProcess()
    {
        UIManager.Instance.FadeOut();
        yield return new WaitForSeconds(0.5f);
        SceneClear();
        SceneStart();
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.FadeIn();
    }
    public void ReturnToTitle()
    {
        GameManager.Instance.GamePaused();
        UI_Caution _uiCaution = UIManager.Instance.ShowPopupUIAt<UI_Caution>(Vector3.zero);
        _uiCaution.SetText("ARE YOU SURE TO BACK TO THE TITLE?");
        _uiCaution.BindYesButton(() => { RestartScene(); });
        _uiCaution.BindNoButton(() => { Debug.Log("게임 재시작"); GameManager.Instance.GameResume(); });
    }
}
