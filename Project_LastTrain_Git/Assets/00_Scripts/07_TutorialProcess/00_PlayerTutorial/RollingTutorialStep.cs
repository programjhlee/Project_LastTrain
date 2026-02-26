using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "RollingTutorialStep", menuName = "Create Tutorial File / RollingTutorial")]
public class RollingTutorialStep : PlayerTutorialStep
{
    [SerializeField] UI_HUDControlGuideStrategyData _rollGuide;
    UI_ControlGuide _uiControlGuide;
    Action _onRollAction;
    public override IEnumerator Run()
    {
        int curCnt = 0;
        int jumpTutorialClearCnt = 3;
        _uiControlGuide = UIManager.Instance.ShowUIHUD<UI_ControlGuide>(_p.transform);
        _uiControlGuide.BindData(_rollGuide);
        Debug.Log("Shift키는 플레이어가 구릅니다! 구르는동안 무적이에요!");
        _onRollAction = () => { curCnt++; };
        _pAction.OnRoll += _onRollAction;
        while (curCnt < jumpTutorialClearCnt)
        {
            _uiControlGuide.UpdatePos();
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
        if (_uiControlGuide != null)
        {
            _uiControlGuide.Hide();
            _uiControlGuide = null;
        }
        if (_onRollAction == null)
        {
            return;
        }
        _pAction.OnRoll -= _onRollAction;
        _onRollAction = null;
    }
}
