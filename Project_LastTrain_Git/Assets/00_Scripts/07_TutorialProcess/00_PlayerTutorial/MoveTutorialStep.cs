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
    WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

    PlayerUIController _uiController;
    Action _onMoveAction;
    

    public override void Bind(TutorialSystem system)
    {
        _p = system.player;
        _pAction = _p.GetComponent<PlayerAction>();
        _uiController = _p.GetComponentInChildren<PlayerUIController>();
        Debug.Log(_uiController);
    }

    public override IEnumerator Run()
    {
        float curDistance = 0;
        float movementTutorialClearDistance = 20f;
        _uiController.ShowControlGuide(_moveGuide);
        Debug.Log("방향키는 플레이어가 움직입니다! 움직여보세요!");
        _onMoveAction = () => { curDistance += 0.3f; };
        _pAction.OnMove += _onMoveAction;
        while (curDistance < movementTutorialClearDistance)
        {
            if (GameManager.Instance.IsPaused())
            {
                yield return null;
                continue;
            }
            yield return null;
        }
        Release();
    }

    public override void Release()
    {
        _uiController.HideControlGuide();
        if(_onMoveAction == null)
        {
            return;
        }
        _pAction.OnMove -= _onMoveAction;
        _onMoveAction = null;
    }
}
