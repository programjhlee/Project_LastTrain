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
    [SerializeField] Image _uiFadeOut;

    public event Action OnTutorialStart;
    public event Action OnGameStart;
    public event Action OnStageStart;
    public event Action OnStageClear;
    public event Action OnAllStageClear;

    public enum GameState
    {
        Title,
        GameStart,
        Tutorial,
        GamePlaying,
        StageStart,
        StageClear,
        GameAllClear,
        GamePaused
    }

    GameState _beforeState;
    public GameState State
    {
        get;
        private set;
    }
    void OnEnable()
    {
        State = GameState.GamePaused;
        _train.OnTrainDestroy += GameOver;
        _platformController.OnDistanceZero += StageClear;
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
        yield return StartCoroutine(_trainAnim.StartTrainAnim(new Vector3(0,-4,0), 5f));
        _trainSound.PlayTrainRunningSound();
        State = GameState.Tutorial;        
        UIManager.Instance.ShowUIAt<UI_TrainHP>(new Vector3(0, -420));
        _tutorialSystem.TutorialStart();
        OnTutorialStart?.Invoke();
    }

    public void GameStart()
    {
        StartCoroutine(GameStartProcess(() =>
        {
            State = GameState.GamePlaying;
            OnGameStart?.Invoke();
        }
       ));
    }

    public void StageStart()
    {
		if (LevelManager.Instance.IsMaxLevel())
		{
			Debug.Log("올클리어! 더이상 진행할 수 없습니다!");
            return;
		}
		StartCoroutine(StageStartProcess(() =>
		{
			State = GameState.GamePlaying;
			OnGameStart?.Invoke();
		}
	   ));
	}

    public IEnumerator StageStartProcess(Action OnComplete)
    {
        State = GameState.StageStart;
        OnStageStart?.Invoke();
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(GameStartProcess(OnComplete));
    }

    public IEnumerator GameStartProcess(Action OnComplete)
    {
        UIManager.Instance.ShowUIAt<UI_StageAnnounce>(new Vector2(0,250));
        yield return new WaitForSeconds(4f);
        OnComplete?.Invoke();
    }
    public void GameAllClear()
    {
        StartCoroutine(GameAllClearProcess());

    }

    public IEnumerator GameAllClearProcess()
    {
        State = GameState.GameAllClear;
        _trainSound.PlayTrainStartSound();
        CameraManager.Instance.AllClearCamProcess();
        yield return new WaitForSeconds(3f);
        UIManager.Instance.FadeOut();
        _trainSound.StopTrainRunningSound();
        Debug.Log("올클리어를 축하드립니다!");
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
        if (LevelManager.Instance.IsMaxLevel())
        {
            OnAllStageClear?.Invoke();
            StartCoroutine(GameAllClearProcess());
            return;
        }
        OnStageClear?.Invoke();
        State = GameState.StageClear;
    }
    public void GameOver()
    {
        Debug.Log("게임오버.....");
        State = GameState.GamePaused;
    }
}
