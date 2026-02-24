using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "InteractionTutorialStep",menuName = "Create Tutorial File/InteractionTutorial")]
public class InteractionTutorialStep : TutorialStep
{
    
    Player _p;
    TrainEventSystem _trainEventSystem; 
    int curCnt = 0;
    int targetCnt = 2;

    public Action _onFixAction;

    public void Bind(Player p,TrainEventSystem trainEventSystem)
    {
        _p = p;
        _trainEventSystem = trainEventSystem;
    }
    public override IEnumerator Run()
    {
        _onFixAction = () => { curCnt++; };
        Debug.Log("열차에 부정적인 영향을 주는 이벤트에요! 없애주세요!");
        Event brokenEvent = _trainEventSystem.SpawnEventAt(0, (int)TrainEventSystem.Events.BROKENEVENT);
        Event bombEvent = _trainEventSystem.SpawnEventAt(3, (int)TrainEventSystem.Events.BOMBEVENT);
        brokenEvent.OnFixed += _onFixAction; 
        bombEvent.OnFixed += _onFixAction; 
        while(curCnt < targetCnt)
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
        if (_onFixAction != null)
        {
            _onFixAction = null;
        }
    }
}
