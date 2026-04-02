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

    public void Awake()
    {
        cam = Camera.main;
        sightOutLeftCnt = 0;
        sightOutRightCnt = 0;

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
            if (ui_eventCautionLeft == null)
            {
                ui_eventCautionLeft = UIManager.Instance.ShowUIAt<UI_EventCaution>(new Vector2(-800, 0), "UI_EventCautionLeft");
            }
            else
            {
                ui_eventCautionLeft.Show();
            }
            ui_eventCautionLeft.SetEventCount(sightOutLeftCnt);
        }
        else
        {
            if (ui_eventCautionLeft == null)
            {
                return;
            }
            ui_eventCautionLeft.Hide();
        }
        if (sightOutRightCnt >= 1)
        {
            if (ui_eventCautionRight == null)
            {
                ui_eventCautionRight = UIManager.Instance.ShowUIAt<UI_EventCaution>(new Vector2(800, 0), "UI_EventCautionRight");
            }
            else
            {
                ui_eventCautionRight.Show();
            }
            ui_eventCautionRight.SetEventCount(sightOutRightCnt);
        }
        else
        {
            if (ui_eventCautionRight == null)
            {
                return;
            }
            ui_eventCautionRight.Hide();
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

        Collider curEventCol = curEvent.GetComponent<Collider>();
        while (curEventCol == null || !curEventCol.enabled || curEventCol.bounds.center == Vector3.zero)
        {
            curEventCol = curEvent.GetComponent<Collider>();
            yield return null;
        }
         
        if (!wasOutLeftEvent.ContainsKey(curEvent) || !wasOutRightEvent.ContainsKey(curEvent))
        {
            if (!wasOutLeftEvent.ContainsKey(curEvent))
            {
                wasOutLeftEvent[curEvent] = CheckOutBound(leftBound, curEventCol);
                if (wasOutLeftEvent[curEvent])
                {
                    sightOutLeftCnt++;
                }
            }
            if (!wasOutRightEvent.ContainsKey(curEvent))
            {
                wasOutRightEvent[curEvent] = CheckOutBound(rightBound, curEventCol);
                if (wasOutRightEvent[curEvent])
                {
                    sightOutRightCnt++;
                }
            }
        }
        else
        {
            bool curEventBoundLeft = CheckOutBound(leftBound, curEventCol);
            bool curEventBoundRight = CheckOutBound(rightBound, curEventCol);

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
        if(ui_eventCautionLeft != null)
        {
            ui_eventCautionLeft.Hide();

        }
        if(ui_eventCautionRight != null)
        {
            ui_eventCautionRight.Hide();
        }

        sightOutRightCnt = 0;
        sightOutLeftCnt = 0;
        wasOutRightEvent.Clear();
        wasOutLeftEvent.Clear();
    }
}
