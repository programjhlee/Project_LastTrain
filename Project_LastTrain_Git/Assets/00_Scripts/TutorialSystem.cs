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

    TutorialContext _tutorialContext;

    public Player player
    {
        get { return _player; }
    }
    public EnemySpawner enemySpawner => _enemySpawner;
    public Train train
    {
        get { return _train; }
    }

    void Start()
    {
        _tutorialContext = new TutorialContext
        {
            train = _train,
            player = _player
        };

        _trainEventSystem = _train.GetComponent<TrainEventSystem>();
        _bigEventSystem = _train.GetComponent<BigEventSystem>();
        _playerAction = _player.GetComponent<PlayerAction>();
        _skipBtn.onClick.AddListener(SkipTutorial);
        _skipBtn.gameObject.SetActive(false);

    }

    public void TutorialStart()
    {
        Debug.Log("튜토리얼 시작!");
        _skipBtn.gameObject.SetActive(true);
        StartCoroutine(AllTutorialProcess());
    }

    IEnumerator AllTutorialProcess()
    {
        for (int i = 0; i < _steps.Count; i++)
        {
            _steps[i].Bind(this);
            yield return StartCoroutine(_steps[i].Run());
            _steps[i].Release();
        }
        GameManager.Instance.GameStart();
        _skipBtn.gameObject.SetActive(false);
    }

    public void SkipTutorial()
    {
        Debug.Log("Tutorial Skip");
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
