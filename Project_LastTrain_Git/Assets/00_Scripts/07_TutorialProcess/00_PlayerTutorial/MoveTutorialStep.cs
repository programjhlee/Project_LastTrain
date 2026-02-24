using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "MoveTutorialStep" , menuName = "Create Tutorial File / MoveTutorial")]
public class MoveTutorialStep : PlayerTutorialStep
{
    Action _onMoveAction;
    public override IEnumerator Run()
    {
        float curDistance = 0;
        float movementTutorialClearDistance = 20f;
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
            Debug.Log($"Distance : {curDistance} / {movementTutorialClearDistance}");
            yield return null;
        }
        Release();
    }

    public override void Release()
    {
        if(_onMoveAction == null)
        {
            return;
        }
        _pAction.OnMove -= _onMoveAction;
        _onMoveAction = null;
    }
}
