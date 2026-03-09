using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "RollingTutorialStep", menuName = "Create Tutorial File / RollingTutorial")]
public class RollingTutorialStep : TutorialStep
{
    [SerializeField] UI_HUDControlGuideStrategyData _rollGuide;
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
        int jumpTutorialClearCnt = 3;
        _uiControlGuide = UIManager.Instance.ShowUIHUD<UI_HUDControlGuide>(_p.transform);
        _uiControlGuide.BindData(_rollGuide);
        Debug.Log("Shift키는 플레이어가 구릅니다! 구르는동안 무적이에요!");
        _onDodgeAction = () => { curCnt++; };
        _pAction.OnDodge += _onDodgeAction;
        while (curCnt < jumpTutorialClearCnt)
        {
            _uiControlGuide.UpdatePos();
            if (GameManager.Instance.IsPaused())
            {
                yield return null;
                continue;
            }
            Debug.Log($"RollCnt : {curCnt} / {jumpTutorialClearCnt}");
            yield return _waitForEndOfFrame;
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
        if (_onDodgeAction == null)
        {
            return;
        }
        _pAction.OnDodge -= _onDodgeAction;
        _onDodgeAction = null;
    }
}
