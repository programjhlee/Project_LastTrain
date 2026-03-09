using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "InteractionTutorialStep",menuName = "Create Tutorial File/InteractionTutorial")]
public class InteractionTutorialStep : TutorialStep
{
    public string ControlGuideName { get; private set; } = "Interaction";
    Player _p;
    TrainEventSystem _trainEventSystem; 
    public Action _onFixAction;

    WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
    public override void Bind(TutorialSystem system)
    {
        _p = system.player;
        _trainEventSystem = system.train.GetComponent<TrainEventSystem>();
    }
    public override IEnumerator Run()
    {
        int curCnt = 0;
        int targetCnt = 2;

        _onFixAction = () => { curCnt++; };
        Debug.Log("열차에 부정적인 영향을 주는 이벤트에요! 없애주세요!");
        Event brokenEvent = _trainEventSystem.SpawnEventAt(0, 0);
        Event bombEvent = _trainEventSystem.SpawnEventAt(0.25f, 1);
        brokenEvent.OnFixed += _onFixAction; 
        bombEvent.OnFixed += _onFixAction;
        Debug.Log(curCnt);
        while (curCnt < targetCnt)
        {
            Debug.Log(curCnt);
            if (GameManager.Instance.IsPaused())
            {
                yield return null;
                continue;
            }
            yield return _waitForEndOfFrame;
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
