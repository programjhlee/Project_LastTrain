using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventData", menuName = "Create TrainEventData")]
public class EventData : ScriptableObject
{
    public int EventID {get ; set;}
    public string EventName { get;  set;}
    public string EventPrefabAddress { get; set; }
    public float CyclePerTime { get; set; }
    public float FixAmount { get; set; }
    public float DamageToTrain { get; set; }

}