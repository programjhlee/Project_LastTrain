using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;
public class GameManager : SingletonManager<GameManager>
{
    [SerializeField] Train _train;
    [SerializeField] TrainAnim _trainAnim;
    [SerializeField] TrainSound _trainSound;
    [SerializeField] TutorialSystem _tutorialSystem;
    [SerializeField] PlatformController _platformController;
    [SerializeField] Button _menuButton;


    public event Action OnTutorialStart;
    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnStageStart;
    public event Action OnStageClear;
    public event Action OnAllStageClear;

    public enum GameState
    {
        Title,
        Tutorial,
        GameStart,
        GamePlaying,
        StageStart,
        StageClear,
        GameAllClear,
        GamePaused,
        GameOver
    }

    GameState _beforeState;
    public GameState State
    {
        get;
        private set;
    }
    void OnEnable()
    {
        State = GameState.Title;
        _train.OnTrainDestroy += GameOver;
        _platformController.OnDistanceZero += StageClear;
        _platformController.OnPlatformArrived += () => _menuButton.gameObject.SetActive(true);
        LevelManager.Instance.OnAllLevelClear += GameAllClear;
    }
    void OnDisable()
    {
        _train.OnTrainDestroy -= GameOver;
        _platformController.OnDistanceZero -= StageClear;
        LevelManager.Instance.OnAllLevelClear -= GameAllClear;
        OnGameStart = null;
        OnTutorialStart = null;
        OnStageClear = null;
    }
    

    public void TutorialStart()
    {
        StartCoroutine(TutorialStartProcess());
    }

    IEnumerator TutorialStartProcess()
    {
        _trainSound.PlayTrainStartSound();
        yield return StartCoroutine(_trainAnim.StartTrainAnim(new Vector3(0,-4,0), 3f));
        _trainSound.PlayTrainRunningSound();
        State = GameState.Tutorial;        
        UIManager.Instance.ShowUIAt<UI_TrainHP>(new Vector3(0, -420));
        UIManager.Instance.ShowUIAt<UI_Coin>(new Vector3(250,-150));
        _menuButton.gameObject.SetActive(true);
        _tutorialSystem.TutorialStart();
        OnTutorialStart?.Invoke();
    }

    public void GameStart()
    {
        State = GameState.GameStart;
        SoundManager.Instance.PlayBGM(SoundManager.BGMType.GameBGM);
        StartCoroutine(GameStartProcess(() =>
        {
            State = GameState.GamePlaying;
            OnGameStart?.Invoke();
        }
       ));
    }

    public void StageStart()
    {
        State = GameState.StageStart;
        StartCoroutine(StageStartProcess(() =>
		{
			State = GameState.GamePlaying;
            OnGameStart?.Invoke();
		}
	   ));
	}

    public IEnumerator StageStartProcess(Action OnComplete)
    {
        _menuButton.gameObject.SetActive(false);
        State = GameState.StageStart;
        OnStageStart?.Invoke();
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(GameStartProcess(OnComplete));
    }

    public IEnumerator GameStartProcess(Action OnComplete)
    {
        SoundManager.Instance.VolumeFadeIn(3f);
        _menuButton.gameObject.SetActive(false);
        UIManager.Instance.ShowUIAt<UI_StageAnnounce>(new Vector2(0,250));
        yield return new WaitForSeconds(4f);
        _menuButton.gameObject.SetActive(true);
        OnComplete?.Invoke();
    }
    public void GameAllClear()
    {
        StartCoroutine(GameAllClearProcess());

    }

    public IEnumerator GameAllClearProcess()
    {
        State = GameState.GameAllClear;
        SoundManager.Instance.VolumeFadeOut();
        _trainSound.PlayTrainStartSound();
        CameraManager.Instance.AllClearCamProcess();
        yield return new WaitForSeconds(4f);
        _trainSound.StopTrainRunningSound();
        UIManager.Instance.FadeOut();
        SoundManager.Instance.StopBGM();
        yield return new WaitForSeconds(1f);
        CutsceneManager.Instance.PlayCutScene(CutsceneManager.CutsceneType.GameClear);
    }

    public void Init()
    {
        State = GameState.Title;
        Time.timeScale = 1f;
    }
    public bool IsGamePlaying()
    {
        return State == GameState.GamePlaying;
    }
    public bool IsTutorial()
    {
        return State == GameState.Tutorial;
    }
    public bool IsPaused()
    {
        return State == GameState.GamePaused;
    }
    public void GamePaused()
    {
        _beforeState = State;

        State = GameState.GamePaused;
    }
    public void GameResume()
    {
        State = _beforeState;
   
    }

    public void StageClear()
    {
        _menuButton.gameObject.SetActive(false);
        if (LevelManager.Instance.IsMaxLevel())
        {
            OnAllStageClear?.Invoke();
            StartCoroutine(GameAllClearProcess());
            return;
        }
        SoundManager.Instance.VolumeFadeOut(3f, 0.2f);
        OnStageClear?.Invoke();
        State = GameState.StageClear;
    }

    public void ResetGameManager()
    {
        StopAllCoroutines();
        State = GameState.Title;
    }
    public void GameOver()
    {
        SoundManager.Instance.StopBGM();
        State = GameState.GameOver;
        OnGameOver?.Invoke();
        StartCoroutine(GameOverProcess());
    }
    IEnumerator GameOverProcess()
    {
        _menuButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(5f);
        UIManager.Instance.FadeOut();
        yield return new WaitForSeconds(2f);
        CutsceneManager.Instance.PlayCutScene(CutsceneManager.CutsceneType.GameOver);
        _menuButton.gameObject.SetActive(true);
    }


}
