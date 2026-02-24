using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "JumpTutorialStep", menuName = "Create Tutorial File / JumpTutorial")]
public class JumpTutorialStep : PlayerTutorialStep
{
    Action _onJumpAction;
    public override IEnumerator Run()
    {
        _pAction = _p.GetComponent<PlayerAction>();
        int curCnt = 0;
        int jumpTutorialClearCnt = 3;
        Debug.Log("Alt키는 플레이어가 점프합니다! 점프해보세요!");
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
        _pAction.OnJump -= _onJumpAction;
        _onJumpAction = null;
    }
}
