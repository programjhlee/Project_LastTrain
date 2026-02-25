using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BigEventTutorial", menuName = "Create Tutorial File/ BigEventTutorial")]
public class BigEventTutorialStep : TutorialStep
{
    Train _train;
    BigEventSystem _bigEventSystem;
    BigEvent _bigEvent;
    Action _onCrashed;
    public void Bind(Train train)
    {
        _train = train;
        _bigEventSystem = _train.GetComponent<BigEventSystem>();
    }
    public override IEnumerator Run()
    {
        int curCnt = 0;
        int targetCnt = 3;
        _onCrashed = () => { curCnt++; };
        _bigEvent = _bigEventSystem.SpawnBigEvent();
        _bigEvent.Init(5f, _train);
        _bigEvent.Damage = 0;
        _bigEvent.OnDestroy += _onCrashed;
        while (curCnt < targetCnt)
        {
            if (!_bigEvent.gameObject.activeSelf)
            {
                _bigEvent = _bigEventSystem.SpawnBigEvent();
                _bigEvent.Init(5f, _train);
                _bigEvent.Damage = 0;
                _bigEvent.OnDestroy += _onCrashed;
            }
            yield return null;
        }
        Release();
    }
    public override void Release()
    {
        if (_onCrashed != null)
        {
            _onCrashed = null;
        }

    }
}
