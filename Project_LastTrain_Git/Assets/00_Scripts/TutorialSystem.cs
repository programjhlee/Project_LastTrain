using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSystem : MonoBehaviour
{
    [SerializeField] Button _skipBtn;
    [SerializeField] List<TutorialStep> _steps;
    [SerializeField] Player _player;
    [SerializeField] EnemySpawner _enemySpawner;
    [SerializeField] Train _train;
    [SerializeField] Switch _switch;

    TrainEventSystem _trainEventSystem;
    BigEventSystem _bigEventSystem;
    PlayerAction _playerAction;




    void Start()
    {
        _trainEventSystem = _train.GetComponent<TrainEventSystem>();
        _bigEventSystem = _train.GetComponent<BigEventSystem>();
        _playerAction = _player.GetComponent<PlayerAction>();
        _skipBtn.gameObject.SetActive(true);
        _skipBtn.onClick.AddListener(SkipTutorial);
        
    }

    public void TutorialStart()
    {
        StartCoroutine(AllTutorialProcess());
    }

    IEnumerator AllTutorialProcess()
    {
        for (int i = 0; i < _steps.Count; i++) 
        {
            switch (_steps[i])
            {
                case PlayerTutorialStep step:
                    step.Bind(_player);
                    yield return StartCoroutine(step.Run());
                    break;
            }
        }
        GameManager.Instance.GameStart();
    }

    public void SkipTutorial()
    {
        StopAllCoroutines();

        for (int i = 0; i < _steps.Count; i++)
        {
            _steps[i].Release();
        }
        _steps.Clear();
        _skipBtn.onClick.RemoveListener(SkipTutorial);
        _skipBtn.gameObject.SetActive(false);
        GameManager.Instance.GameStart();
        gameObject.SetActive(false);
    }
}
