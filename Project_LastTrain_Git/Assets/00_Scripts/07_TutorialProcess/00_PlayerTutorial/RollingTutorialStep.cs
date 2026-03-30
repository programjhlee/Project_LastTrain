using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "RollingTutorialStep", menuName = "Create Tutorial File / RollingTutorial")]
public class RollingTutorialStep : TutorialStep
{
    [SerializeField] AnnounceStrategyQuest _uiAnnounceStrategy;
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

    }
    public override IEnumerator Run()
    {
        int curCnt = 0;
        int rollTutorialClearCnt = 3;

        _onDodgeAction = () => { curCnt++; };
        _pAction.OnDodge += _onDodgeAction;

        _uiAnnounce = UIManager.Instance.ShowAnnounce(
           _uiAnnounceStrategy,
          $"TO Dodge \n<size=20> Dodge {curCnt:D2} / {rollTutorialClearCnt:D2}</size>",
           new Vector2(0, 300f)
           );
        _uiAnnounce.Init();
        _uiAnnounce.SetQuestSprite(_dodgeKeySprite);

        while (curCnt < rollTutorialClearCnt)
        {
            if (GameManager.Instance.IsPaused())
            {
                yield return null;
                continue;
            }
            yield return _waitForEndOfFrame;
            _uiAnnounce.SetAnnounceText($"TO DODGE  \n<size=20>  DODGE COUNT {curCnt:D2} / {rollTutorialClearCnt:D2}</size>");
        }
        _uiAnnounce.SetAnnounceText($"TO DODGE  \n<size=20>  DODGE COUNT {curCnt:D2} / {rollTutorialClearCnt:D2}</size>");
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
        if (_uiAnnounce != null)
        {
            _uiAnnounce.Hide();
        }
    }
}
