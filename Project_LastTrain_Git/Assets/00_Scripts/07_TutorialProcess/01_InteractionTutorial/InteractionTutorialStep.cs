using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "InteractionTutorialStep",menuName = "Create Tutorial File/InteractionTutorial")]
public class InteractionTutorialStep : TutorialStep
{
    [SerializeField] AnnounceStrategyQuest _uiAnnounceStrategy;
    [SerializeField] Sprite _interactionSprite;
    Event _brokenEvent;
    Event _bombEvent;
    UIHUDController _brokenEventHUD;
    UIHUDController _bombEventHUD;
    TrainEventSystem _trainEventSystem;
    UI_Announce _uiAnnounce;
    public Action _onFixAction;
    bool _isCanceled;
    WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
    public override void Bind(TutorialSystem system)
    {
        _trainEventSystem = system.train.GetComponent<TrainEventSystem>();
        _isCanceled = false;
    }
    public override IEnumerator Run()
    {
        int curCnt = 0;
        int targetCnt = 2;
        _uiAnnounce = UIManager.Instance.ShowAnnounce(
        _uiAnnounceStrategy,
       $"TO FIX \n<size=20> FIX {curCnt:D2} / {targetCnt:D2}</size>",
        new Vector2(0, 300f)
        );
        _uiAnnounce.Init();
        _uiAnnounce.SetQuestSprite(_interactionSprite);

        _onFixAction = () => { curCnt++; };
        _brokenEvent = _trainEventSystem.SpawnEventAt(0, 0);
        _bombEvent = _trainEventSystem.SpawnEventAt(0.25f, 1);
        _brokenEventHUD = _brokenEvent.GetComponent<UIHUDController>();
        _bombEventHUD = _bombEvent.GetComponent<UIHUDController>();
        _brokenEvent.OnFixed += _onFixAction; 
        _bombEvent.OnFixed += _onFixAction;
        while (curCnt < targetCnt && !_isCanceled)
        {
            if (GameManager.Instance.IsPaused())
            {
                yield return null;
                continue;
            }
            _brokenEventHUD.UpdateUIHUDPos();
            _bombEventHUD.UpdateUIHUDPos();
            _uiAnnounce.SetAnnounceText($"TO FIX \n<size=20> FIX {curCnt:D2} / {targetCnt:D2}</size>");
            yield return _waitForEndOfFrame;
        }
        _uiAnnounce.SetAnnounceText($"TO FIX \n<size=20> FIX {curCnt:D2} / {targetCnt:D2}</size>");
        _uiAnnounce.QuestClear();
    }
    public override void Release()
    {
        _isCanceled = true;
        if (_brokenEvent != null && _brokenEvent.gameObject.activeSelf)
        {
            _brokenEvent.Exit();
            _brokenEventHUD.UIHUDListClear();
        }
        if (_bombEvent != null && _bombEvent.gameObject.activeSelf)
        {
            _bombEvent.Exit();
            _bombEventHUD.UIHUDListClear();
        }
        if (_onFixAction != null)
        {
            _onFixAction = null;
        }
        if (_uiAnnounce != null)
        {
            _uiAnnounce.Hide();
        }
    }
}
