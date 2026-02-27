using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "JumpTutorialStep", menuName = "Create Tutorial File / JumpTutorial")]
public class JumpTutorialStep : PlayerTutorialStep
{
    [SerializeField] UI_HUDControlGuideStrategyData _jumpGuide;
    UI_ControlGuide _uiControlGuide;
    UIHUDStack _uiController;
    Action _onJumpAction;
    public override IEnumerator Run()
    {
        _uiControlGuide = UIManager.Instance.ShowUIHUD<UI_ControlGuide>(_p.transform);
        _uiControlGuide.BindData(_jumpGuide);
        _uiController = _p.GetComponent<UIHUDStack>();
        _uiController.AddUIHUD(_uiControlGuide);
        _pAction = _p.GetComponent<PlayerAction>();
        int curCnt = 0;
        int jumpTutorialClearCnt = 3;
        Debug.Log("Alt키는 플레이어가 점프합니다! 점프해보세요!");
        _onJumpAction = () => { curCnt++; };
        _pAction.OnJump += _onJumpAction;
        while (curCnt < jumpTutorialClearCnt)
        {
            _uiController.UpdateUIHUDPos();
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
        if (_uiControlGuide != null)
        {
            _uiControlGuide.Hide();
            _uiControlGuide = null;
        }
            
        if (_onJumpAction == null)
        {
            return;
        }
        _pAction.OnJump -= _onJumpAction;
        _onJumpAction = null;
    }
}
