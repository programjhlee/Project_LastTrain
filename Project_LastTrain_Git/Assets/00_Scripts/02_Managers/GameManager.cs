using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : SingletonManager<GameManager>
{
    [SerializeField] Train _train;
    [SerializeField] TutorialSystem _tutorialSystem;
    [SerializeField] PlatformController _platformController;

    public event Action OnTutorialStart;
    public event Action OnGameStart;
    public event Action OnStageStart;
    public event Action OnStageClear;
    public event Action OnAllStageClear;

    public enum GameState
    {
        GameStart,
        Tutorial,
        GamePlaying,
        StageStart,
        StageClear,
        GamePaused
    }

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
    }
    void OnDisable()
    {
        _train.OnTrainDestroy -= GameOver;
        _platformController.OnDistanceZero -= StageClear;
        OnGameStart = null;
        OnTutorialStart = null;
        OnStageClear = null;
    }
    public void TutorialStart()
    {
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
        State = GameState.GamePaused;
    }
    public void GameResume()
    {
        State = GameState.GamePlaying;
    }
    public void StageClear()
    {
        if (LevelManager.Instance.IsMaxLevel())
        {
            OnAllStageClear?.Invoke();
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
