using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JumpTutorialStep : TutorialStep
{
    PlayerAction _pAction;
    Action _onJumpAction;

    public override void Bind(Player player)
    {
        _pAction = player.GetComponent<PlayerAction>();
    }
    public override IEnumerator Run()
    {
        float curCnt = 0;
        float jumpTutorialClearCnt = 20f;
        Debug.Log("Alt키는 플레이어가 움직입니다! 움직여보세요!");
        _onJumpAction = () => { curCnt += 0.3f; };
        _pAction.OnJump += _onJumpAction;
        while (curCnt < jumpTutorialClearCnt)
        {
            if (GameManager.Instance.IsPaused())
            {
                yield return null;
                continue;
            }
            Debug.Log($"Distance : {curCnt} / {jumpTutorialClearCnt}");
            yield return null;
        }
        Release();
    }
    public override void Release()
    {
        if (_onJumpAction == null)
        {
            return;
        }
        _pAction.OnMove -= _onJumpAction;
        _onJumpAction = null;
    }
}
