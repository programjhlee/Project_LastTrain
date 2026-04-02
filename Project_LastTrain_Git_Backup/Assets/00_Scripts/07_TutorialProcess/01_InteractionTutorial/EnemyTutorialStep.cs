using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EnemyTutorialStep", menuName ="Create Tutorial File/EnemyTutorial")]
public class EnemyTutorialStep : TutorialStep
{
    [SerializeField] AnnounceStrategyQuest _uiAnnounceStrategy;
    [SerializeField] Sprite _attackKeySprite;
    UI_Announce _uiAnnounce;
    EnemySpawner _enemySpawner;
    List<Enemy> _enemyList;
    public Action OnDied;
    bool _isCanceled;
    public override void Bind(TutorialSystem system)
    {
        _enemySpawner = system.enemySpawner;
        _isCanceled = false;
    }

    public override IEnumerator Run()
    {
        _enemyList = new List<Enemy>();
        int curCnt = 0;
        int targetCnt = 3;

        _uiAnnounce = UIManager.Instance.ShowAnnounce(
          _uiAnnounceStrategy,
         $"TO ATTACK \n<size=20> DEFEAT ENEMY {curCnt:D2} / {targetCnt:D2}</size>",
          new Vector2(0, 300f)
          );
        _uiAnnounce.Init();
        _uiAnnounce.SetQuestSprite(_attackKeySprite);

        for (int i = 0; i < targetCnt; i++)
        {
            Enemy enemy = _enemySpawner.SpawnEnemy();
            _enemyList.Add(enemy);
        }
        while (curCnt < targetCnt && !_isCanceled)
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
                _enemyList[i].OnUpdate();
            }
            _uiAnnounce.SetAnnounceText($"TO ATTACK \n<size=20> DEFEAT ENEMY {curCnt:D2} / {targetCnt:D2}</size>");
            yield return null;
        }
        _uiAnnounce.SetAnnounceText($"TO ATTACK \n<size=20> DEFEAT ENEMY {curCnt:D2} / {targetCnt:D2}</size>");
        _uiAnnounce.QuestClear();
    }

    public override void Release()
    {
        if (_enemyList != null)
        {
            for (int i = 0; i < _enemyList.Count; i++)
            {
                _enemyList[i].OnDespawn();
            }
            _enemyList.Clear();
        }
        if (_uiAnnounce != null)
        {
            _uiAnnounce.Hide();
        }
    }
}
