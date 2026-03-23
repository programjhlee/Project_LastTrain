using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "InteractionTutorialStep",menuName = "Create Tutorial File/InteractionTutorial")]
public class InteractionTutorialStep : TutorialStep
{
    [SerializeField] AnnounceStrategyQuest _uiAnnounceStrategy;
    [SerializeField] Sprite _interactionSprite;
    TrainEventSystem _trainEventSystem;
    UI_Announce _uiAnnounce;
    public Action _onFixAction;

    WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
    public override void Bind(TutorialSystem system)
    {
        _uiAnnounce = UIManager.Instance.ShowUI<UI_Announce>();
        _uiAnnounce.Init();
        _uiAnnounce.SetUIStrategy(_uiAnnounceStrategy);
        _uiAnnounce.SetQuestSprite(_interactionSprite);
        _trainEventSystem = system.train.GetComponent<TrainEventSystem>();
    }
    public override IEnumerator Run()
    {
        int curCnt = 0;
        int targetCnt = 2;

        _onFixAction = () => { curCnt++; };
        Event brokenEvent = _trainEventSystem.SpawnEventAt(0, 0);
        Event bombEvent = _trainEventSystem.SpawnEventAt(0.25f, 1);
        UIHUDController brokenEventHUD = brokenEvent.GetComponent<UIHUDController>();
        UIHUDController bombEventHUD = bombEvent.GetComponent<UIHUDController>();
        brokenEvent.OnFixed += _onFixAction; 
        bombEvent.OnFixed += _onFixAction;
        while (curCnt < targetCnt)
        {
            if (GameManager.Instance.IsPaused())
            {
                yield return null;
                continue;
            }
            brokenEventHUD.UpdateUIHUDPos();
            bombEventHUD.UpdateUIHUDPos();
            _uiAnnounce.SetAnnounceText($"TO FIX \r\n<size=25> FIX {curCnt:D2} / {targetCnt:D2}</size>");
            yield return _waitForEndOfFrame;
        }
        _uiAnnounce.SetAnnounceText($"TO FIX \r\n<size=25> FIX {curCnt:D2} / {targetCnt:D2}</size>");
        _uiAnnounce.QuestClear();
    }
    public override void Release()
    {
        if (_onFixAction != null)
        {
            _onFixAction = null;
        }
        _uiAnnounce.Hide();
    }
}
