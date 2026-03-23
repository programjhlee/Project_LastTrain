using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "RollingTutorialStep", menuName = "Create Tutorial File / RollingTutorial")]
public class RollingTutorialStep : TutorialStep
{
    [SerializeField] AnnounceStrategyQuest _announce;
    [SerializeField] Sprite _dodgeKeySprite;
    UI_Announce _uiAnnounce;
    Player _p;
    PlayerAction _pAction;

    UI_HUDControlGuide _uiControlGuide;
    Action _onDodgeAction;

    WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

    public override void Bind(TutorialSystem system)
    {
        _p = system.player;
        _pAction = _p.GetComponent<PlayerAction>();
        _uiAnnounce = UIManager.Instance.ShowUIAt<UI_Announce>(new Vector2(0, 300f));
        _uiAnnounce.Init();
        _uiAnnounce.SetUIStrategy(_announce);
        _uiAnnounce.SetQuestSprite(_dodgeKeySprite);
    }
    public override IEnumerator Run()
    {
        int curCnt = 0;
        int rollTutorialClearCnt = 3;

        _onDodgeAction = () => { curCnt++; };
        _pAction.OnDodge += _onDodgeAction;

        while (curCnt < rollTutorialClearCnt)
        {
            if (GameManager.Instance.IsPaused())
            {
                yield return null;
                continue;
            }
            yield return _waitForEndOfFrame;
            _uiAnnounce.SetAnnounceText($"TO DODGE  \r\n<size=25>  DODGE COUNT {curCnt:D2} / {rollTutorialClearCnt:D2}</size>");
        }
        _uiAnnounce.SetAnnounceText($"TO DODGE  \r\n<size=25>  DODGE COUNT {curCnt:D2} / {rollTutorialClearCnt:D2}</size>");
        _uiAnnounce.QuestClear();
    }
    public override void Release()
    {
        if (_onDodgeAction == null)
        {
            return;
        }
        _pAction.OnDodge -= _onDodgeAction;
        _onDodgeAction = null;
        _uiAnnounce.Hide();
    }
}
