using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices.ComTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public class TrainEventSystem : MonoBehaviour
{
    [SerializeField] Train _train;
    [SerializeField] GameObject _trainBack;
    [SerializeField] List<EventData> _eventDataList;
    
    EventSightChecker _eventSightChecker;
    Renderer _trainRend;
    
    int[] _eventID;
    List<Dictionary<string, object>> _eventLevelSpawnTimeData;
    List<Dictionary<string, object>> _trainEventData;

    Dictionary<int, EventData> _eventDataDics = new Dictionary<int, EventData>();
    Dictionary<int, Stack<Event>> _inactiveEventPools = new Dictionary<int, Stack<Event>>();

    List<Event> _executeEvents = new List<Event>();
    List<Event> _endEvents = new List<Event>();

    float _eventSpawnTime;
    float _curTime;
    
    void Awake()
    {
        _eventLevelSpawnTimeData = DataManager.Instance.GetData(DataManager.DataTables.EventSpawnData);
        _trainEventData = DataManager.Instance.GetData(DataManager.DataTables.TrainEventData);

        _eventSightChecker = GetComponent<EventSightChecker>();
        _trainRend = _trainBack.GetComponent<Renderer>();
        _eventID = new int[_eventDataList.Count];

        for (int i = 0; i < _trainEventData.Count; i++)
        {
            _eventDataList[i].EventID = int.Parse(_trainEventData[i]["EVENTID"].ToString());
            _eventDataList[i].EventName = _trainEventData[i]["EVENTNAME"].ToString();
            _eventDataList[i].CyclePerTime = float.Parse(_trainEventData[i]["CYCLEPERTIME"].ToString());
            _eventDataList[i].DamageToTrain = float.Parse(_trainEventData[i]["DAMAGETOTRAIN"].ToString());
            _eventDataList[i].FixAmount = float.Parse(_trainEventData[i]["FIXAMOUNT"].ToString()) + (float.Parse(_trainEventData[i]["FIXAMOUNTPERLEVEL"].ToString()) * LevelManager.Instance.Level);
            _eventDataDics[_eventDataList[i].EventID] = _eventDataList[i];
            _eventID[i] = _eventDataList[i].EventID;

            Transform eventPool = new GameObject($"{_eventDataDics[_eventID[i]].EventName}").GetComponent<Transform>();
            Stack<Event> eventPoolStack = new Stack<Event>();
            
            for (int j = 0; j < 20; j++)
            {
                Event poolEvent = Instantiate(Resources.Load<GameObject>(_trainEventData[i]["PATH"].ToString())).GetComponent<Event>();
                SoundManager.Instance.AddObjAudioSource(poolEvent.gameObject);
                poolEvent.gameObject.name = $"{_eventDataDics[_eventID[i]]}_{j + 1}";
                poolEvent.gameObject.SetActive(false);
                poolEvent.transform.SetParent(eventPool);
                eventPoolStack.Push(poolEvent);
            }
            _inactiveEventPools[_eventID[i]] = eventPoolStack;

        }
    }

    public void OnEnable()
    {
        GameManager.Instance.OnStageClear += ResetTrainEventSystem;
        GameManager.Instance.OnAllStageClear += ResetTrainEventSystem;
        LevelManager.Instance.OnLevelChanged += SetEventLevelData;
        _train.OnTrainDestroy += ResetTrainEventSystem;
        _train.OnReset += ResetTrainEventSystem;
        _train.OnReset += SetEventLevelData;
    }

    public void OnDisable()
    {
        GameManager.Instance.OnStageClear -= ResetTrainEventSystem;
        GameManager.Instance.OnAllStageClear -= ResetTrainEventSystem;
        LevelManager.Instance.OnLevelChanged -= SetEventLevelData;
        _train.OnTrainDestroy -= ResetTrainEventSystem;
        _train.OnReset -= ResetTrainEventSystem;
        _train.OnReset -= SetEventLevelData;
    }
    public void Update()
    {
        if (!GameManager.Instance.IsGamePlaying() || !_train.IsRunning)
        {
            return;
        }

        _curTime += Time.deltaTime;
        if(_curTime >= _eventSpawnTime)
        {
            _curTime = 0;
            SpawnEventRandomPos();
        }
        EventExecute();
        EventClear();
    }

    public Event SpawnEventAt(float normalizeXPos, int? eventIdx = null)
    {
        int selectIdx;

        Event evt;
        Renderer evtRend;

        if (eventIdx == null)
        {
            selectIdx = UnityEngine.Random.Range(0, _eventID.Length);
        }
        else
        {
            selectIdx = eventIdx.Value;
        }
        evt = _inactiveEventPools[_eventID[selectIdx]].Pop();
        evtRend = evt.GetComponent<Renderer>();
        
        float xPos = _trainRend.bounds.center.x + _trainRend.bounds.extents.x * normalizeXPos;
        float yPos = _trainRend.bounds.center.y + _trainRend.bounds.extents.y + evtRend.bounds.extents.y;

        evt.gameObject.SetActive(true);
        evt.Enter(_eventDataDics[_eventID[selectIdx]], xPos, yPos);
        BindTrainEvent(evt);
        _executeEvents.Add(evt);

        return evt;
    }

    public void BindTrainEvent(Event evt)
    {
        UIHUDController _evtUIHUDStack = evt.GetComponent<UIHUDController>();
        UI_HUDValueBar _uiEventFixValueBar = UIManager.Instance.ShowUIHUD<UI_HUDValueBar>(evt.transform);
        _uiEventFixValueBar.transform.localScale = new Vector3(1, 1, 1);
        evt.OnTakeFix += _uiEventFixValueBar.SetValue;
        evt.OnFixed += _evtUIHUDStack.UIHUDListClear;
        _evtUIHUDStack.AddUIHUD(_uiEventFixValueBar);
        if(evt.TryGetComponent<ITrainDamageEvent>(out ITrainDamageEvent trainDamageEvent))
        {
            trainDamageEvent.OnDamage += _train.TakeDamage;
        }
    }
    public Event SpawnEventRandomPos()
    {
        float rndXPos = UnityEngine.Random.Range(-0.8f, 0.8f);
        return SpawnEventAt(rndXPos);
    }

    public void EventExecute()
    {
        for (int i = 0; i < _executeEvents.Count; i++)
        {
            if (!_executeEvents[i].gameObject.activeSelf)
            {
                _eventSightChecker.CheckEventSight(_executeEvents[i]);
                _endEvents.Add(_executeEvents[i]);
                _inactiveEventPools[_executeEvents[i].EventData.EventID].Push(_executeEvents[i]);
            }
            else
            {
                _executeEvents[i].Execute();
                _eventSightChecker.CheckEventSight(_executeEvents[i]);
            }
        }
    }
    public void EventClear()
    {
        for(int i = 0; i < _endEvents.Count; i++)
        {
            _eventSightChecker.CheckEventSight(_endEvents[i]);
            _executeEvents.Remove(_endEvents[i]);
        }
        _endEvents.Clear();
    }
    public void ResetTrainEventSystem()
    {
        _curTime = 0;

        for (int i = 0; i < _executeEvents.Count; i++)
        {
            if (_executeEvents[i].gameObject.activeSelf)
            {
                if (_executeEvents[i].TryGetComponent<ITrainDamageEvent>(out ITrainDamageEvent trainDamageEvent))
                {
                    trainDamageEvent.OnDamage -= _train.TakeDamage;
                }
                _executeEvents[i].Exit();
                _endEvents.Add(_executeEvents[i]);
            }
        }
        _eventSightChecker.SightCheckerClear();
    }
    public void SetEventLevelData()
    {
        for(int i = 0; i < _eventLevelSpawnTimeData.Count; i++)
        {
            if (int.Parse(_eventLevelSpawnTimeData[i]["LEVEL"].ToString()) == LevelManager.Instance.Level)
            {
                _eventSpawnTime = float.Parse(_eventLevelSpawnTimeData[i]["EVENTINTERVAL"].ToString());
            }
        }
    }
}
