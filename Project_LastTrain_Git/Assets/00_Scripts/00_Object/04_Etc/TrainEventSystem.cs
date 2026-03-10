using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices.ComTypes;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;
public class TrainEventSystem : MonoBehaviour
{

    [SerializeField] Train train;
    [SerializeField] List<EventData> eventDataList;
    [SerializeField] PlatformController platformController;
    [SerializeField] GameObject trainBack;
    [SerializeField] UI_HUDValueBarStrategyData _uiEventFixBarData;
    
    EventSightChecker eventSightChecker;
    Renderer rend;
    
    int[] eventID;
    List<Dictionary<string, object>> eventLevelSpawnTimeData;
    List<Dictionary<string, object>> trainEventData;

    Dictionary<int, EventData> eventDataDics = new Dictionary<int, EventData>();
    Dictionary<int, Stack<Event>> inactiveEventPools = new Dictionary<int, Stack<Event>>();

    List<Event> executeEvents = new List<Event>();
    List<Event> endEvents = new List<Event>();

    float eventSpawnTime;
    float curTime;
    void Start()
    {
        eventLevelSpawnTimeData = DataManager.Instance.GetData((int)Define.DataTables.EventSpawnData);
        trainEventData = DataManager.Instance.GetData((int)Define.DataTables.TrainEventData);

        eventSightChecker = GetComponent<EventSightChecker>();
        rend = trainBack.GetComponent<Renderer>();
        eventID = new int[eventDataList.Count];

        for (int i = 0; i < trainEventData.Count; i++)
        {
            eventDataList[i].EventID = int.Parse(trainEventData[i]["EVENTID"].ToString());
            eventDataList[i].EventName = trainEventData[i]["EVENTNAME"].ToString();
            eventDataList[i].CyclePerTime = float.Parse(trainEventData[i]["CYCLEPERTIME"].ToString());
            eventDataList[i].DamageToTrain = float.Parse(trainEventData[i]["DAMAGETOTRAIN"].ToString());
            eventDataList[i].FixAmount = float.Parse(trainEventData[i]["FIXAMOUNT"].ToString()) + (float.Parse(trainEventData[i]["FIXAMOUNTPERLEVEL"].ToString()) * LevelManager.Instance.Level);
            eventDataDics[eventDataList[i].EventID] = eventDataList[i];
            eventID[i] = eventDataList[i].EventID;

            Transform eventPool = new GameObject($"{eventDataDics[eventID[i]].EventName}").GetComponent<Transform>();
            Stack<Event> eventPoolStack = new Stack<Event>();
            for (int j = 0; j < 20; j++)
            {
                Event poolEvent = Instantiate(Resources.Load<GameObject>(trainEventData[i]["PATH"].ToString())).GetComponent<Event>();
                poolEvent.gameObject.name = $"{eventDataDics[eventID[i]]}_{j + 1}";
                poolEvent.gameObject.SetActive(false);
                poolEvent.transform.SetParent(eventPool);
                eventPoolStack.Push(poolEvent);
            }
            inactiveEventPools[eventID[i]] = eventPoolStack;
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
        }
        EventExecute();
        EventClear();
    }


    public Event SpawnEvent(int? eventIdx = null)
    {
        int selectIdx;

        Event evt;

        if (eventIdx == null)
        {
            selectIdx = UnityEngine.Random.Range(0, eventID.Length);
        }
        else
        {
            selectIdx = eventIdx.Value;
        }
        evt = inactiveEventPools[eventID[selectIdx]].Pop();
        evt.gameObject.SetActive(true);
        evt.Enter(eventDataDics[eventID[selectIdx]]);
        BindEvent(evt);
        executeEvents.Add(evt);

        return evt;
    }

    public Event SpawnEventAt(float normalizeXPos, int? eventIdx = null)
    {
        Event evt;
        Renderer evtRend;

        evt = SpawnEvent(eventIdx);
        evtRend = evt.GetComponent<Renderer>();
        
        float xPos = rend.bounds.center.x + rend.bounds.extents.x * normalizeXPos;
        float yPos = rend.bounds.center.y + rend.bounds.extents.y + evtRend.bounds.extents.y;

        evt.transform.position = new Vector3(xPos, yPos);
        return evt;
    }

    public void BindEvent(Event evt)
    {
        UIHUDController _evtUIHUDStack = evt.GetComponent<UIHUDController>();
        UI_HUDValueBar _uiEventFixValueBar = UIManager.Instance.ShowUIHUD<UI_HUDValueBar>(evt.transform);
        _uiEventFixValueBar.transform.localScale = new Vector3(1, 1, 1);
        evt.OnTakeFix += _uiEventFixValueBar.SetValue;
        evt.OnFixed += _evtUIHUDStack.UIHUDListClear;
        _evtUIHUDStack.AddUIHUD(_uiEventFixValueBar);
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
            if (!executeEvents[i].gameObject.activeSelf)
            {
                eventSightChecker.CheckEventSight(executeEvents[i]);
                endEvents.Add(executeEvents[i]);
                inactiveEventPools[executeEvents[i].EventData.EventID].Push(executeEvents[i]);
            }
            else
            {
                executeEvents[i].Execute();
                eventSightChecker.CheckEventSight(executeEvents[i]);
            }
            
        }
    }

    public void EventClear()
    {
        for(int i = 0; i < endEvents.Count; i++)
        {
            executeEvents.Remove(endEvents[i]);
        }
        endEvents.Clear();
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
                endEvents.Add(executeEvents[i]);
            }
        }
        EventClear();
        executeEvents.Clear();
        eventSightChecker.SightCheckerClear();
        curTime = 0;
    }
    public void SetEventLevelData()
    {
        for(int i = 0; i < eventLevelSpawnTimeData.Count; i++)
        {
            if (int.Parse(eventLevelSpawnTimeData[i]["LEVEL"].ToString()) == LevelManager.Instance.Level)
            {
                eventSpawnTime = float.Parse(eventLevelSpawnTimeData[i]["EVENTINTERVAL"].ToString());
            }
        }
    }
}
