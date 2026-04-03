using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EventSightChecker : MonoBehaviour
{
    Camera _cam;

    UI_EventCaution _ui_eventCautionLeft;
    UI_EventCaution _ui_eventCautionRight;

    Dictionary<Event,bool> _wasOutLeftEvent = new Dictionary<Event,bool>();
    Dictionary<Event, bool> _wasOutRightEvent = new Dictionary<Event, bool>();
    int _sightOutLeftCnt;
    int _sightOutRightCnt;

    public void Awake()
    {
        _cam = Camera.main;
        _sightOutLeftCnt = 0;
        _sightOutRightCnt = 0;

    }
    public void CheckEventSight(Event curEvent)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_cam);
        Plane[] leftBound = new Plane[] { planes[0], planes[4], planes[5] };
        Plane[] rightBound = new Plane[] { planes[1], planes[4], planes[5] };

        StartCoroutine(CheckOutBoundProcess(leftBound, rightBound, curEvent));
    }
    public void ShowCautionUI()
    {
        if (_sightOutLeftCnt >= 1)
        {
            if (_ui_eventCautionLeft == null)
            {
                _ui_eventCautionLeft = UIManager.Instance.ShowUIAt<UI_EventCaution>(new Vector2(-800, 0), "UI_EventCautionLeft");
            }
            else
            {
                _ui_eventCautionLeft.Show();
            }
            _ui_eventCautionLeft.SetEventCount(_sightOutLeftCnt);
        }
        else
        {
            if (_ui_eventCautionLeft == null)
            {
                return;
            }
            _ui_eventCautionLeft.Hide();
        }
        if (_sightOutRightCnt >= 1)
        {
            if (_ui_eventCautionRight == null)
            {
                _ui_eventCautionRight = UIManager.Instance.ShowUIAt<UI_EventCaution>(new Vector2(800, 0), "UI_EventCautionRight");
            }
            else
            {
                _ui_eventCautionRight.Show();
            }
            _ui_eventCautionRight.SetEventCount(_sightOutRightCnt);
        }
        else
        {
            if (_ui_eventCautionRight == null)
            {
                return;
            }
            _ui_eventCautionRight.Hide();
        }
    }

    public bool CheckOutBound(Plane[] planes, Collider col)
    {
        return !GeometryUtility.TestPlanesAABB(planes,col.bounds);
    }

    IEnumerator CheckOutBoundProcess(Plane[] leftBound, Plane[] rightBound, Event curEvent)
    {
        if (!curEvent.gameObject.activeSelf)
        {
            if (!_wasOutLeftEvent.TryGetValue(curEvent,out bool wasLeftOutValue) || !_wasOutRightEvent.TryGetValue(curEvent, out bool wasRightOutValue))
            {
                yield break;
            }

            if (_wasOutLeftEvent[curEvent])
            {
                _sightOutLeftCnt--;
            }
            if (_wasOutRightEvent[curEvent])
            {
                _sightOutRightCnt--;
            }
            _wasOutLeftEvent.Remove(curEvent);
            _wasOutRightEvent.Remove(curEvent);
            ShowCautionUI();
            yield break;
        }

        Collider curEventCol = curEvent.GetComponent<Collider>();
        while (curEventCol == null || !curEventCol.enabled || curEventCol.bounds.center == Vector3.zero)
        {
            curEventCol = curEvent.GetComponent<Collider>();
            yield return null;
        }
         
        if (!_wasOutLeftEvent.ContainsKey(curEvent) || !_wasOutRightEvent.ContainsKey(curEvent))
        {
            if (!_wasOutLeftEvent.ContainsKey(curEvent))
            {
                _wasOutLeftEvent[curEvent] = CheckOutBound(leftBound, curEventCol);
                if (_wasOutLeftEvent[curEvent])
                {
                    _sightOutLeftCnt++;
                }
            }
            if (!_wasOutRightEvent.ContainsKey(curEvent))
            {
                _wasOutRightEvent[curEvent] = CheckOutBound(rightBound, curEventCol);
                if (_wasOutRightEvent[curEvent])
                {
                    _sightOutRightCnt++;
                }
            }
        }
        else
        {
            bool curEventBoundLeft = CheckOutBound(leftBound, curEventCol);
            bool curEventBoundRight = CheckOutBound(rightBound, curEventCol);

            if (_wasOutLeftEvent[curEvent] != curEventBoundLeft)
            {
                if (_wasOutLeftEvent[curEvent])
                {
                    _sightOutLeftCnt--;
                }
                else
                {
                   _sightOutLeftCnt++;
                }
                _wasOutLeftEvent[curEvent] = curEventBoundLeft;
            }
            if (_wasOutRightEvent[curEvent] != curEventBoundRight)
            {
                if (_wasOutRightEvent[curEvent])
                {
                    _sightOutRightCnt--;
                }
                else
                {
                    _sightOutRightCnt++;
                }
                _wasOutRightEvent[curEvent] = curEventBoundRight;
            }
        }
        ShowCautionUI();
    }
    public void SightCheckerClear()
    {
        if(_ui_eventCautionLeft != null)
        {
            _ui_eventCautionLeft.Hide();

        }
        if(_ui_eventCautionRight != null)
        {
            _ui_eventCautionRight.Hide();
        }

        _sightOutRightCnt = 0;
        _sightOutLeftCnt = 0;
        _wasOutRightEvent.Clear();
        _wasOutLeftEvent.Clear();
    }
}
