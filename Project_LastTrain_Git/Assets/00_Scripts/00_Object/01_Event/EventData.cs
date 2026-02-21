using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventData", menuName = "Event/CreateEventData")]
public class EventData : ScriptableObject
{
    public int eventID;
    public string eventName;
    public float cyclePerTime;
    public float fixAmount;
    public float damageToTrain;
}