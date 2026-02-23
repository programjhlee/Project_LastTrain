using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EventSightChecker : MonoBehaviour
{
    Camera cam;

    UI_EventCaution ui_eventCautionLeft;
    UI_EventCaution ui_eventCautionRight;

    Dictionary<Event,bool> wasInLeftEvent = new Dictionary<Event,bool>();
    Dictionary<Event, bool> wasInRightEvent = new Dictionary<Event, bool>();

    int sightOutLeftCnt;
    int sightOutRightCnt;


    public void Start()
    {
        cam = Camera.main;
        ui_eventCautionLeft = UIManager.Instance.ShowUIAt<UI_EventCaution>(new Vector2(-800, 0),"UI_EventCautionLeft");
        ui_eventCautionRight = UIManager.Instance.ShowUIAt<UI_EventCaution>(new Vector2(800, 0), "UI_EventCautionRight");
        ui_eventCautionLeft.Hide();
        ui_eventCautionRight.Hide();

    }
    public void CheckEventSight(List<Event> eventList)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        Plane[] leftBound = new Plane[] { planes[0] };
        Plane[] rightBound = new Plane[] { planes[1] };

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


        if (eventList.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < eventList.Count; i++)
        {
            if (eventList[i] == null)
            {
                if (wasInLeftEvent.ContainsKey(eventList[i]))
                {
                    if (!wasInLeftEvent[eventList[i]])
                    {
                        sightOutLeftCnt--;
                    }
                    wasInLeftEvent.Remove(eventList[i]);
                }
                if (wasInRightEvent.ContainsKey(eventList[i]))
                {
                    if (!wasInRightEvent[eventList[i]])
                    {
                        sightOutRightCnt--;
                    }
                    wasInRightEvent.Remove(eventList[i]);
                }
                return;
            }
            if (!wasInLeftEvent.ContainsKey(eventList[i]) && !wasInRightEvent.ContainsKey(eventList[i]))
            {
                wasInLeftEvent[eventList[i]] = CheckInBound(leftBound, eventList[i].transform);
                wasInRightEvent[eventList[i]] = CheckInBound(rightBound, eventList[i].transform);

                if (!wasInLeftEvent[eventList[i]])
                {
                    sightOutLeftCnt++;
                }
                if (!wasInRightEvent[eventList[i]])
                {
                    sightOutRightCnt++;
                }
            }
            else
            {
                if (wasInLeftEvent[eventList[i]] != CheckInBound(leftBound, eventList[i].transform))
                {
                    if (wasInLeftEvent[eventList[i]])
                    {
                        sightOutLeftCnt++;
                    }
                    else
                    {
                        sightOutLeftCnt--;
                    }
                    wasInLeftEvent[eventList[i]] = CheckInBound(leftBound, eventList[i].transform);
                }
                if (wasInRightEvent[eventList[i]] != CheckInBound(rightBound, eventList[i].transform))
                {
                    if (wasInRightEvent[eventList[i]])
                    {
                        sightOutRightCnt++;
                    }
                    else
                    {
                        sightOutRightCnt--;
                    }
                    wasInRightEvent[eventList[i]] = CheckInBound(rightBound, eventList[i].transform);
                }
            }
        }
    }

    public bool CheckInBound(Plane[] planes, Transform target)
    {  
        Collider col = target.GetComponent<Collider>();   
        return GeometryUtility.TestPlanesAABB(planes,col.bounds);

    }
}
