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

    bool _isSkip;

    TrainEventSystem _trainEventSystem;
    BigEventSystem _bigEventSystem;
    PlayerAction _playerAction;
    public Player player
    {
        get { return _player; }
    }
    public EnemySpawner enemySpawner => _enemySpawner;
    public Train train
    {
        get { return _train; }
    }
    private void Awake()
    {
        _trainEventSystem = _train.GetComponent<TrainEventSystem>();
        _bigEventSystem = _train.GetComponent<BigEventSystem>();
        _playerAction = _player.GetComponent<PlayerAction>();
        _isSkip = false;
    }

    public void ResetTutorialSystem()
    {
        _isSkip = false;
        ResetTutorial();
        gameObject.SetActive(true);
        _skipBtn.gameObject.SetActive(false);
    }


    public void TutorialStart()
    {
        _skipBtn.gameObject.SetActive(true);
        _skipBtn.onClick.AddListener(SkipTutorial);
        StartCoroutine(AllTutorialProcess());
    }

    IEnumerator AllTutorialProcess()
    {
        for (int i = 0; i < _steps.Count; i++)
        {
            if (_isSkip)
            {
                yield break;
            }
            _steps[i].Bind(this);
            yield return StartCoroutine(_steps[i].Run());
            yield return new WaitForSeconds(2f);
            _steps[i].Release();
            yield return new WaitForSeconds(0.5f);
        }
        GameManager.Instance.GameStart();
        _skipBtn.gameObject.SetActive(false);
    }

    public void SkipTutorial()
    {
        _isSkip = true;
        for (int i = 0; i < _steps.Count; i++)
        {
            _steps[i].Release();
        }
        _skipBtn.onClick.RemoveListener(SkipTutorial);
        _skipBtn.gameObject.SetActive(false);
        GameManager.Instance.GameStart();
    }

    public void ResetTutorial()
    {
        StopAllCoroutines();
        for (int i = 0; i < _steps.Count; i++)
        {
            _steps[i].Release();
        }
        _skipBtn.onClick.RemoveListener(SkipTutorial);
        _skipBtn.gameObject.SetActive(false);
    }
}
