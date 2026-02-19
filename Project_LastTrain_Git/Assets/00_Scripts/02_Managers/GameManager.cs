using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : SingletonManager<GameManager>
{
    [SerializeField] Train train;
    [SerializeField] PlatformController platformController;

    public event Action OnGameStart;
    public event Action OnStageClear;

    public enum GameState
    {
        GameReady,
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
        train.OnTrainDestroy += GameOver;
        platformController.OnArrived += StageClear;
    }
    void OnDisable()
    {
        train.OnTrainDestroy -= GameOver;
        platformController.OnArrived -= StageClear;
        OnGameStart = null;
    }

    public void GameStart()
    {
        if (LevelManager.Instance.IsMaxLevel())
        {
            Debug.Log("올클리어! 더이상 진행할 수 없습니다!");
        }
        State = GameState.GamePlaying;
        UIManager.Instance.ShowUIAt<UI_TrainHP>(new Vector3(0, -420)); 
        OnGameStart?.Invoke();
    }
    public bool IsGamePlaying()
    {
        return State == GameState.GamePlaying;
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
