using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "RollingTutorialStep", menuName = "Create Tutorial File / RollingTutorial")]
public class RollingTutorialStep : PlayerTutorialStep
{
    Action _onRollAction;
    public override IEnumerator Run()
    {
       
        int curCnt = 0;
        int jumpTutorialClearCnt = 3;
        Debug.Log("Shift키는 플레이어가 구릅니다! 구르는동안 무적이에요!");
        _onRollAction = () => { curCnt++; };
        _pAction.OnRoll += _onRollAction;
        while (curCnt < jumpTutorialClearCnt)
        {
            if (GameManager.Instance.IsPaused())
            {
                yield return null;
                continue;
            }
            Debug.Log($"RollCnt : {curCnt} / {jumpTutorialClearCnt}");
            yield return null;
        }
        Release();
    }
    public override void Release()
    {
        if (_onRollAction == null)
        {
            return;
        }
        _pAction.OnRoll -= _onRollAction;
        _onRollAction = null;
    }
}
