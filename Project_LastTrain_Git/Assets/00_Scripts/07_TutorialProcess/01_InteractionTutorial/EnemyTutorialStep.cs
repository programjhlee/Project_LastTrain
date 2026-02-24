using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EnemyTutorialStep", menuName ="Create Tutorial File/EnemyTutorial")]
public class EnemyTutorialStep : TutorialStep
{
    EnemySpawner _enemySpawner;
    List<Enemy> _enemyList = new List<Enemy>();
    int curCnt = 0;
    int targetCnt = 3;
    public Action OnDied;
    public void Bind(EnemySpawner enemySpawner)
    {
        _enemySpawner = enemySpawner;
    }

    public override IEnumerator Run()
    {
        _enemySpawner.Init();
        for (int i = 0; i < targetCnt; i++)
        {
            _enemyList.Add(_enemySpawner.SpawnEnemy());
            Debug.Log(_enemyList[i]);
            yield return null; 
        }
        int enemyIdx = 0;
        while (_enemyList.Count > 0)
        {
            int remainCnt = _enemyList.Count;
            if (_enemyList[enemyIdx] == null)
            {
                _enemyList.RemoveAt(enemyIdx);
                continue;
            }
            _enemyList[enemyIdx].OnUpdate();
            enemyIdx = (enemyIdx + 1) % remainCnt;
            yield return null;
        }
        Release();
    }

    public override void Release()
    {
        _enemyList.Clear();
    }

}
