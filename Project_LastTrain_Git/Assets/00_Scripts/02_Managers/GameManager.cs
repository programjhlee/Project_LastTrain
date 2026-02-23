using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : SingletonManager<GameManager>
{
    [SerializeField] Train _train;
    [SerializeField] TutorialSystem _tutorialSystem;
    [SerializeField] PlatformController _platformController;

    public event Action OnGameStart;
    public event Action OnStageClear;

    public enum GameState
    {
        GameReady,
        Tutorial,
        GamePlaying,
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
        _platformController.OnArrived += StageClear;
    }
    void OnDisable()
    {
        _train.OnTrainDestroy -= GameOver;
        _platformController.OnArrived -= StageClear;
        OnGameStart = null;
    }
    public void TutorialStart()
    {
        State = GameState.Tutorial;
        UIManager.Instance.ShowUIAt<UI_TrainHP>(new Vector3(0, -420));
        _tutorialSystem.TutorialStart();
    }
    public void GameStart()
    {
        if (LevelManager.Instance.IsMaxLevel())
        {
            Debug.Log("올클리어! 더이상 진행할 수 없습니다!");
        }
        State = GameState.GamePlaying;
        OnGameStart?.Invoke();
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
        OnStageClear?.Invoke();
        State = GameState.GamePaused;
    }
    public void GameOver()
    {
        Debug.Log("게임오버.....");
        State = GameState.GamePaused;
    }
}
