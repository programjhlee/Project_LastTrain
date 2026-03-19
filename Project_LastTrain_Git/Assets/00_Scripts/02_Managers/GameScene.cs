using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField] Image _fadeOutImage;
    [SerializeField] Train _train;
    [SerializeField] TrainEventSystem _trainEventSystem;
    [SerializeField] BigEventSystem _bigEventSystem;
    [SerializeField] PlatformController _platformController;
    [SerializeField] EnemySpawner _enemySpawner;
    [SerializeField] UI_Title _uiTitle;
    [SerializeField] TutorialSystem _tutorialSystem;
    
    [SerializeField] Button _startButton;

    
    public void Awake()
    {
        Init();
        _fadeOutImage.gameObject.SetActive(false);
    }

    public void Init()
    {
        CameraManager.Instance.Init();
        LevelManager.Instance.Init();
        _uiTitle.Show();
        _startButton.gameObject.SetActive(true);
        _train.Init();
        _trainEventSystem.Init();
        _bigEventSystem.Init();
        _platformController.Init();
        _platformController.OnPlatformRunning += () => UIManager.Instance.ShowUIAt<UI_Distance>(new Vector2(0,350f));
        _platformController.OnDistanceZero += () => UIManager.Instance.HideUI<UI_Distance>();
        _platformController.OnReset += () => UIManager.Instance.HideUI<UI_Distance>();
        
        _tutorialSystem.Init();
    }
    public void SceneStart()
    {
        GameManager.Instance.Init();
        Init();
    }
    public void SceneClear()
    {
        GameManager.Instance.GamePaused();
        _train.ResetTrain();
        _platformController.ResetPlatform();
        _trainEventSystem.ResetTrainEventSystem();
        _trainEventSystem.ReleaseEvents();
        _bigEventSystem.ResetBigEventSystem();
    }

    public void RestartScene()
    {
        StartCoroutine(RestartProcess());
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator RestartProcess()
    {
        _fadeOutImage.gameObject.SetActive(true);
        SceneClear();
        SceneStart();
        yield return new WaitForSeconds(1f);
        _fadeOutImage.gameObject.SetActive(false);
    }
}
