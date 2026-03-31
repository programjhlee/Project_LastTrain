using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BigEventTutorial", menuName = "Create Tutorial File/ BigEventTutorial")]
public class BigEventTutorialStep : TutorialStep
{
    [SerializeField] AnnounceStrategyQuest _announceQuest;
    [SerializeField] Sprite _shieldSprite;
    Train _train;
    BigEventSystem _bigEventSystem;
    BigEvent _bigEvent;
    Action _onCrashed;
    UI_Announce _uiAnnounce;
    bool _isCanceled;
    public override void Bind(TutorialSystem system)
    {
        _train = system.train;
        _bigEventSystem = _train.GetComponent<BigEventSystem>();
        _isCanceled = false;
    }
    public override IEnumerator Run()
    {
        int curCnt = 0;
        int targetCnt = 3;
        _uiAnnounce = UIManager.Instance.ShowAnnounce(
            _announceQuest, $"<size=30> PROTECT TRAIN </size> \n <size=25> {curCnt} / {targetCnt} </size>"
            , new Vector2(0, 150f));
        _uiAnnounce.SetQuestSprite(_shieldSprite);
        _onCrashed = () => { curCnt++; };
        while (curCnt < targetCnt&& !_isCanceled)
        {
            _uiAnnounce.SetAnnounceText($"<size=30> PROTECT TRAIN </size> \n <size=25> {curCnt} / {targetCnt} </size>");
            if (_bigEvent == null||(_bigEvent != null && _bigEvent.gameObject.activeSelf == false))
            {
                yield return new WaitForSeconds(2f);
                _bigEvent = _bigEventSystem.SpawnBigEvent();
                _bigEvent.Damage = 0;
                _bigEvent.OnDestroy += _onCrashed;
            }
            yield return null;
        }
        _uiAnnounce.SetAnnounceText($"<size=30> PROTECT TRAIN </size> \n <size=25> {curCnt} / {targetCnt} </size>");
        _uiAnnounce.QuestClear();
    }
    public override void Release()
    {
        _isCanceled = true;
        if (_uiAnnounce != null)
        {
            _uiAnnounce.Hide();
        }
        if (_onCrashed != null)
        {
            _onCrashed = null;
        }
        if(_bigEvent != null)
        {
            _bigEvent.gameObject.SetActive(false);
        }
    }
}
