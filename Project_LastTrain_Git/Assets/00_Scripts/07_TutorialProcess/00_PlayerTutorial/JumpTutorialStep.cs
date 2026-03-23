using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "JumpTutorialStep", menuName = "Create Tutorial File / JumpTutorial")]
public class JumpTutorialStep : TutorialStep
{
    [SerializeField] AnnounceStrategyQuest _uiAnnounceStrategy;
    [SerializeField] Sprite _jumpKeySprite; 
    UI_Announce _uiAnnounce;
    Player _p;
    PlayerAction _pAction;
    Action _onJumpAction;
    WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

    public override void Bind(TutorialSystem system)
    {
        _p = system.player;
        _pAction = _p.GetComponent<PlayerAction>();
        _uiAnnounce = UIManager.Instance.ShowUIAt<UI_Announce>(new Vector2(0, 300f));
        _uiAnnounce.Init();
        _uiAnnounce.SetUIStrategy(_uiAnnounceStrategy);
        _uiAnnounce.SetQuestSprite(_jumpKeySprite);
    }


    public override IEnumerator Run()
    {
        int curCnt = 0;
        int jumpTutorialClearCnt = 3;
        _onJumpAction = () => { curCnt++; };
        _pAction.OnJump += _onJumpAction;
        while (curCnt < jumpTutorialClearCnt)
        {
            if (GameManager.Instance.IsPaused())
            {
                yield return null;
                continue;
            }
            _uiAnnounce.SetAnnounceText($"TO JUMP \r\n<size=25> JUMP {curCnt:D2} / {jumpTutorialClearCnt:D2}</size>");
            yield return _waitForEndOfFrame;
        }
        _uiAnnounce.SetAnnounceText($"TO JUMP \r\n<size=25> JUMP {curCnt:D2} / {jumpTutorialClearCnt:D2}</size>");
        _uiAnnounce.QuestClear();
    }
    public override void Release()
    {
        if (_onJumpAction == null)
        {
            return;
        }
        _pAction.OnJump -= _onJumpAction;
        _onJumpAction = null;
        _uiAnnounce.Hide();
    }
}
