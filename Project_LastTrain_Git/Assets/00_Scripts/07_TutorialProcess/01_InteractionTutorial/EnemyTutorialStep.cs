using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EnemyTutorialStep", menuName ="Create Tutorial File/EnemyTutorial")]
public class EnemyTutorialStep : TutorialStep
{
    [SerializeField] AnnounceStrategyQuest _announceStrategyQuest;
    [SerializeField] Sprite _attackKeySprite;
    UI_Announce _uiAnnounce;
    EnemySpawner _enemySpawner;
    List<Enemy> _enemyList;
    public Action OnDied;
    WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
    public override void Bind(TutorialSystem system)
    {
        _uiAnnounce = UIManager.Instance.ShowPopupUIAt<UI_Announce>(new Vector2(0, 300f));
        _uiAnnounce.Init();
        _uiAnnounce.SetUIStrategy(_announceStrategyQuest);
        _uiAnnounce.SetQuestSprite(_attackKeySprite);
        _enemySpawner = system.enemySpawner;
        
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
                _enemyList[i].OnUpdate();
            }
            _uiAnnounce.SetAnnounceText($"TO ATTACK \r\n<size=25> DEFEAT ENEMY {curCnt:D2} / {targetCnt:D2}</size>");
            yield return _waitForEndOfFrame;
        }
        _uiAnnounce.SetAnnounceText($"TO ATTACK \r\n<size=25> DEFEAT ENEMY {curCnt:D2} / {targetCnt:D2}</size>");
        _uiAnnounce.QuestClear();
    }

    public override void Release()
    {
        _enemyList.Clear();
        if (_uiAnnounce != null)
        {
            _uiAnnounce.Hide();
        }
    }
}
