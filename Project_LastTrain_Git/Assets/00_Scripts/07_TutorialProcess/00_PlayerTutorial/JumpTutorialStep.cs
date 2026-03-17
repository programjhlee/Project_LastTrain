using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "JumpTutorialStep", menuName = "Create Tutorial File / JumpTutorial")]
public class JumpTutorialStep : TutorialStep
{
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
    }


    public override IEnumerator Run()
    {
        int curCnt = 0;
        int jumpTutorialClearCnt = 3;
        _uiAnnounce.SetQuestText("점프하기");
        _onJumpAction = () => { curCnt++; };
        _pAction.OnJump += _onJumpAction;
        while (curCnt < jumpTutorialClearCnt)
        {
            if (GameManager.Instance.IsPaused())
            {
                yield return null;
                continue;
            }
            Debug.Log($"JumpCnt : {curCnt} / {jumpTutorialClearCnt}");
            yield return _waitForEndOfFrame;
        }
        Release();
    }
    public override void Release()
    {
        if (_onJumpAction == null)
        {
            return;
        }
        _pAction.OnJump -= _onJumpAction;
        _onJumpAction = null;
    }
}
