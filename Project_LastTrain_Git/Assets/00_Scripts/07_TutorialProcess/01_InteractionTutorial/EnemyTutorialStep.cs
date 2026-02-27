using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EnemyTutorialStep", menuName ="Create Tutorial File/EnemyTutorial")]
public class EnemyTutorialStep : TutorialStep
{
    EnemySpawner _enemySpawner;
    List<Enemy> _enemyList;
    public Action OnDied;
    public void Bind(EnemySpawner enemySpawner)
    {
        _enemySpawner = enemySpawner;
        
    }

    public override IEnumerator Run()
    {
        _enemyList = new List<Enemy>();
        int curCnt = 0;
        int targetCnt = 3;
        for (int i = 0; i < targetCnt; i++)
        {
            Enemy enemy = _enemySpawner.SpawnEnemy();
            _enemyList.Add(enemy);
        }
        yield return null;
        while (curCnt < targetCnt)
        {
            if (GameManager.Instance.IsPaused())
            {
                yield return null;
                continue;
            }
            yield return null;
            for (int i = 0; i < _enemyList.Count; i++)
            {
                if (!_enemyList[i].gameObject.activeSelf)
                {
                    _enemyList.RemoveAt(i);
                    curCnt++;
                    yield return null;
                    continue;
                }
                //_enemyList[i].OnUpdate();
            }
            yield return null;
        }
        //Release();
    }

    public override void Release()
    {
        _enemyList.Clear();
    }

}
