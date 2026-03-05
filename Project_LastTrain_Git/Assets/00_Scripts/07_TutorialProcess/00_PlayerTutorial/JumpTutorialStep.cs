using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "JumpTutorialStep", menuName = "Create Tutorial File / JumpTutorial")]
public class JumpTutorialStep : TutorialStep
{
    [SerializeField] UI_HUDControlGuideStrategyData _jumpGuide;
    
    Player _p;
    PlayerAction _pAction;
    
    UI_ControlGuide _uiControlGuide;
    UIHUDController _uiController;
    
    Action _onJumpAction;
    WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

    public override void Bind(TutorialSystem system)
    {
        _p = system.player;
        _pAction = _p.GetComponent<PlayerAction>();
    }


    public override IEnumerator Run()
    {
        _uiControlGuide = UIManager.Instance.ShowUIHUD<UI_ControlGuide>(_p.transform);
        _uiControlGuide.BindData(_jumpGuide);
        _uiController = _p.GetComponent<UIHUDController>();
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
            
        if (_onJumpAction == null)
        {
            return;
        }
        _pAction.OnJump -= _onJumpAction;
        _onJumpAction = null;
    }
}
