using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "MoveTutorialStep" , menuName = "Create Tutorial File / MoveTutorial")]
public class MoveTutorialStep : TutorialStep
{
    [SerializeField] UI_HUDControlGuideStrategyData _moveGuide;
    Player _p;
    PlayerAction _pAction;


    UI_ControlGuide _uiControlGuide;
    UI_ControlGuide _uiControlGuide2;
    UIHUDController _uiController;
    Action _onMoveAction;
    

    public override void Bind(TutorialSystem system)
    {
        _p = system.player;
        _pAction = _p.GetComponent<PlayerAction>();
    }

    public override IEnumerator Run()
    {
        _uiControlGuide = UIManager.Instance.ShowUIHUD<UI_ControlGuide>(_p.transform);
        _uiControlGuide.BindData(_moveGuide);
        _uiControlGuide2 = UIManager.Instance.ShowUIHUD<UI_ControlGuide>(_p.transform);
        _uiControlGuide2.BindData(_moveGuide);
        _uiController = _p.GetComponent<UIHUDController>();
        float curDistance = 0;
        float movementTutorialClearDistance = 20f;
        _uiController.AddUIHUD(_uiControlGuide);
        _uiController.AddUIHUD(_uiControlGuide2);
        Debug.Log("방향키는 플레이어가 움직입니다! 움직여보세요!");
        _onMoveAction = () => { curDistance += 0.3f; };
        _pAction.OnMove += _onMoveAction;
        while (curDistance < movementTutorialClearDistance)
        {
            _uiController.UpdateUIHUDPos();
            if (GameManager.Instance.IsPaused())
            {
                yield return null;
                continue;
            }
            Debug.Log($"Distance : {curDistance} / {movementTutorialClearDistance}");
            yield return null;
        }
        Release();
    }

    public override void Release()
    {
        _uiController.UIHUDListClear();
        _uiControlGuide = null;
        if(_onMoveAction == null)
        {
            return;
        }
        _pAction.OnMove -= _onMoveAction;
        _onMoveAction = null;
    }
}
