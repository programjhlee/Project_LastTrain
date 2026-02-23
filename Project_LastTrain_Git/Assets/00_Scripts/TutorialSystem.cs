using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialSystem : MonoBehaviour
{

    [SerializeField] List<TutorialStep> steps;
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
    }

    public void TutorialStart()
    {
        StartCoroutine(AllTutorialProcess());
    }

    IEnumerator AllTutorialProcess()
    {
        for (int i = 0; i < steps.Count; i++) 
        {
            steps[i].Bind(_player);
            yield return StartCoroutine(steps[i].Run());
            steps.Remove(steps[i]);
        }
        GameManager.Instance.GameStart();
    }

    public void SkipTutorial()
    {
        StopAllCoroutines();

        for (int i = 0; i < steps.Count; i++)
        {
            steps[i].Release();
        }
        GameManager.Instance.GameStart();
        gameObject.SetActive(false);
    }
}
