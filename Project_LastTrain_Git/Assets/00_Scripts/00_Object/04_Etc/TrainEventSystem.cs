using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using System;
using System.Runtime.InteropServices.ComTypes;
public class TrainEventSystem : MonoBehaviour
{
    public enum Events
    {
        BROKENEVENT,
        BOMBEVENT
    }

    [SerializeField] List<EventData> eventDatas;
    [SerializeField] PlatformController platformController;
    [SerializeField] string[] events;
    [SerializeField] Train train;
    [SerializeField] GameObject trainBack;
    EventSightChecker eventSightChecker;
    Renderer rend;
    
    List<Dictionary<string, object>> eventSpawnData;
    List<Dictionary<string, object>> trainEventData;
    Dictionary<string, EventData> eventDataDic = new Dictionary<string, EventData>();
    Dictionary<string, GameObject> eventPrefabs = new Dictionary<string, GameObject>();

    List<Event> executeEvents = new List<Event>();
    List<Event> endEvents = new List<Event>();

    float eventSpawnTime;
    float curTime;
    void Start()
    {
        eventSpawnData = DataManager.Instance.GetData((int)Define.DataTables.EventSpawnData);
        trainEventData = DataManager.Instance.GetData((int)Define.DataTables.TrainEventData);

        eventSightChecker = GetComponent<EventSightChecker>();

        events = Enum.GetNames(typeof(Events));
        rend = trainBack.GetComponent<Renderer>();
        
        for (int i = 0; i < eventDatas.Count; i++)
        {
            for(int j = 0; j < trainEventData.Count; j++)
            {
                if (eventDatas[i].name == trainEventData[j]["EVENTNAME"].ToString())
                {
                    eventDatas[i].eventID = int.Parse(trainEventData[j]["EVENTID"].ToString());
                    eventDatas[i].eventName = trainEventData[j]["EVENTNAME"].ToString();
                    eventDatas[i].cyclePerTime = float.Parse(trainEventData[j]["CYCLEPERTIME"].ToString());
                    eventDatas[i].damageToTrain = float.Parse(trainEventData[j]["DAMAGETOTRAIN"].ToString());
                    eventDatas[i].fixAmount = float.Parse(trainEventData[j]["FIXAMOUNT"].ToString()) + float.Parse(trainEventData[j]["FIXAMOUNTPERLEVEL"].ToString()) * LevelManager.Instance.Level;
                    
                    eventPrefabs.Add(eventDatas[i].name,Resources.Load<GameObject>(trainEventData[j]["PATH"].ToString()));
                    eventDataDic.Add(eventDatas[i].name, eventDatas[i]);
                }
            }
        }
    }

    void OnEnable()
    {
        GameManager.Instance.OnGameStart += SetEventLevelData;
        platformController.OnArrived += ResetTrainEventSystem;
    }
    void OnDisable()
    {
        GameManager.Instance.OnGameStart -= SetEventLevelData;
        platformController.OnArrived -= ResetTrainEventSystem;
    }
    public void Update()
    {
        if (GameManager.Instance.IsPaused() || GameManager.Instance.IsTutorial())
        {
            return;
        }

        curTime += Time.deltaTime;
        if(curTime >= eventSpawnTime)
        {
            curTime = 0;
            Event curEvent = SpawnEventRandomPos();
            BindDamageEvent(curEvent);
        }
        EventExecute();
        endEvents.Clear();
    }

    public Event SpawnEventAt(float normalizeXPos,int eventIdx = -1)
    {
        int rnd = UnityEngine.Random.Range(0, events.Length);
        Event evt;
        Renderer evtRend;

        if(eventIdx < 0) 
        { 
            evt = Instantiate(eventPrefabs[events[rnd]]).GetComponent<Event>();
            evtRend = evt.GetComponent<Renderer>();
        }
        else
        {
            evt = Instantiate(eventPrefabs[events[eventIdx]]).GetComponent<Event>();
            evtRend = evt.GetComponent<Renderer>();
        }
        
        float xPos = rend.bounds.center.x + rend.bounds.extents.x * normalizeXPos;
        float yPos = rend.bounds.center.y + rend.bounds.extents.y + evtRend.bounds.extents.y;

        evt.transform.position = new Vector3(xPos, yPos);

        evt.Enter(eventDataDic[events[rnd]]);
        executeEvents.Add(evt);
        return evt;
    }

    public void BindDamageEvent(Event evt)
    {
        if(evt.TryGetComponent<ITrainDamageEvent>(out ITrainDamageEvent trainDamageEvent))
        {
            trainDamageEvent.OnDamage += train.TakeDamage;
        }
    }

    public Event SpawnEventRandomPos()
    {
        float rndXPos = UnityEngine.Random.Range(-0.8f, 0.8f);
        return SpawnEventAt(rndXPos);
    }

    public void EventExecute()
    {
        for (int i = 0; i < executeEvents.Count; i++)
        {
            if (executeEvents[i] == null)
            {
                eventSightChecker.CheckEventSight(executeEvents[i]);
                endEvents.Add(executeEvents[i]);
                executeEvents.RemoveAt(i);
            }
            else
            {
                executeEvents[i].Execute();
                eventSightChecker.CheckEventSight(executeEvents[i]);
            }
        }
    }
    public void ResetTrainEventSystem()
    {
        for(int i = 0; i< executeEvents.Count; i++)
        {
            if (executeEvents[i] != null)
            {
                if (executeEvents[i].TryGetComponent<ITrainDamageEvent>(out ITrainDamageEvent trainDamageEvent))
                {
                    trainDamageEvent.OnDamage -= train.TakeDamage;
                }
                executeEvents[i].gameObject.SetActive(false);
                

            }
        }
        executeEvents.Clear();
        endEvents.Clear();
        eventSightChecker.SightCheckerClear();
        curTime = 0;

    }
    public void SetEventLevelData()
    {
        for(int i = 0; i < eventSpawnData.Count; i++)
        {
            if (int.Parse(eventSpawnData[i]["LEVEL"].ToString()) == LevelManager.Instance.Level)
            {
                eventSpawnTime = float.Parse(eventSpawnData[i]["EVENTINTERVAL"].ToString());
            }
        }
    }
}
