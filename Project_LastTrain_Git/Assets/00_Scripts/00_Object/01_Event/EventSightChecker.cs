using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EventSightChecker : MonoBehaviour
{
    Camera cam;

    UI_EventCaution ui_eventCautionLeft;
    UI_EventCaution ui_eventCautionRight;

    Dictionary<Event,bool> wasOutLeftEvent = new Dictionary<Event,bool>();
    Dictionary<Event, bool> wasOutRightEvent = new Dictionary<Event, bool>();
    int sightOutLeftCnt;
    int sightOutRightCnt;


    public void Start()
    {
        cam = Camera.main;
        sightOutLeftCnt = 0;
        sightOutRightCnt = 0;

        ui_eventCautionLeft = UIManager.Instance.ShowUIAt<UI_EventCaution>(new Vector2(-800, 0),"UI_EventCautionLeft");
        ui_eventCautionRight = UIManager.Instance.ShowUIAt<UI_EventCaution>(new Vector2(800, 0), "UI_EventCautionRight");
        ui_eventCautionLeft.Hide();
        ui_eventCautionRight.Hide();

    }
    public void CheckEventSight(Event curEvent)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        Plane[] leftBound = new Plane[] { planes[0], planes[4], planes[5] };
        Plane[] rightBound = new Plane[] { planes[1], planes[4], planes[5] };
     

        StartCoroutine(CheckOutBoundProcess(leftBound, rightBound, curEvent));
    }
    public void ShowCautionUI()
    {
        if (sightOutLeftCnt >= 1)
        {
            ui_eventCautionLeft.Show();
            ui_eventCautionLeft.SetEventCount(sightOutLeftCnt);
        }
        else
        {
            ui_eventCautionLeft.Hide();
        }
        if (sightOutRightCnt >= 1)
        {
            ui_eventCautionRight.Show();
            ui_eventCautionRight.SetEventCount(sightOutRightCnt);
        }
        else
        {
            ui_eventCautionRight.Hide();
        }
    }

    public bool CheckOutBound(Plane[] planes, Transform target)
    {
        Collider col = target.GetComponent<Collider>();
        return !GeometryUtility.TestPlanesAABB(planes,col.bounds);

    }

    IEnumerator CheckOutBoundProcess(Plane[] leftBound, Plane[] rightBound, Event curEvent)
    {
        if (curEvent == null)
        {
            if (!wasOutLeftEvent.TryGetValue(curEvent,out bool wasLeftOutValue) || !wasOutRightEvent.TryGetValue(curEvent, out bool wasRightOutValue))
            {
                yield break;
            }

            if (wasOutLeftEvent[curEvent])
            {
                sightOutLeftCnt--;
            }
            if (wasOutRightEvent[curEvent])
            {
                sightOutRightCnt--;
            }
            wasOutLeftEvent.Remove(curEvent);
            wasOutRightEvent.Remove(curEvent);
            ShowCautionUI();
            yield break;
        }

        Collider col = curEvent.GetComponent<Collider>();
        while (col == null || !col.enabled || col.bounds.center == Vector3.zero)
        {
            col = curEvent.GetComponent<Collider>();
            yield return null;
        }
         
        if (!wasOutLeftEvent.ContainsKey(curEvent) || !wasOutRightEvent.ContainsKey(curEvent))
        {
            if (!wasOutLeftEvent.ContainsKey(curEvent))
            {
                wasOutLeftEvent[curEvent] = CheckOutBound(leftBound, curEvent.transform);
                Debug.Log($"Out Left : {wasOutLeftEvent[curEvent]}");
                if (wasOutLeftEvent[curEvent])
                {
                    sightOutLeftCnt++;
                }
            }
            if (!wasOutRightEvent.ContainsKey(curEvent))
            {
                wasOutRightEvent[curEvent] = CheckOutBound(rightBound, curEvent.transform);
                Debug.Log($"Out Right : {wasOutRightEvent[curEvent]}");
                if (wasOutRightEvent[curEvent])
                {
                    sightOutRightCnt++;
                }
            }
        }
        else
        {
            bool curEventBoundLeft = CheckOutBound(leftBound, curEvent.transform);
            bool curEventBoundRight = CheckOutBound(rightBound, curEvent.transform);

            if (wasOutLeftEvent[curEvent] != curEventBoundLeft)
            {
                if (wasOutLeftEvent[curEvent])
                {
                    sightOutLeftCnt--;
                }
                else
                {
                    sightOutLeftCnt++;
                }
                wasOutLeftEvent[curEvent] = curEventBoundLeft;
            }
            if (wasOutRightEvent[curEvent] != curEventBoundRight)
            {
                if (wasOutRightEvent[curEvent])
                {
                    sightOutRightCnt--;
                }
                else
                {
                    sightOutRightCnt++;
                }
                wasOutRightEvent[curEvent] = curEventBoundRight;
            }
        }
        ShowCautionUI();
    }
    public void SightCheckerClear()
    {
        sightOutRightCnt = 0;
        sightOutLeftCnt = 0;
        wasOutRightEvent.Clear();
        wasOutLeftEvent.Clear();
        ui_eventCautionLeft.Hide();
        ui_eventCautionRight.Hide();
    }
}
