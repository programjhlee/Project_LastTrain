using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatformController : MonoBehaviour
{
    [SerializeField] Train _train;
    BackGroundController _backGroundController;
    List<Dictionary<string, object>> platformDataTable;
    float _trainSpeed;
    float _platformDistance;
    bool _trainDestroy;

    public event Action OnPlatformRunning;

    public float PlatformDistance
    {
        get
        {
            return _platformDistance;
        }
        private set
        {
            _platformDistance = value;
        }
    }
    public float TrainSpeed
    {
        get
        {
            return _trainSpeed;
        }
        private set
        {
            _trainSpeed = value;
        }
    }
    public event Action<float> OnDistanceChanged;
    public event Action OnReset;
    public event Action OnDistanceZero;
    public event Action OnPlatformArrived;

    public void Awake()
    {
        platformDataTable = DataManager.Instance.GetData((int)Define.DataTables.PlatformData);
        _backGroundController = GetComponent<BackGroundController>();
    }

    public void OnEnable()
    {
        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnTutorialStart += SetPlatformData;
        GameManager.Instance.OnStageClear += Arrived;
        GameManager.Instance.OnAllStageClear += ResetPlatform;
        LevelManager.Instance.OnLevelChanged += SetPlatformData;
        _train.OnTrainDestroy += TrainDestroy;
    }

    public void OnDisable()
    {
        GameManager.Instance.OnGameStart -= OnGameStart;
        GameManager.Instance.OnTutorialStart -= SetPlatformData;
        GameManager.Instance.OnAllStageClear -= ResetPlatform;
        GameManager.Instance.OnStageClear -= Arrived;
        LevelManager.Instance.OnLevelChanged -= SetPlatformData;
        _train.OnTrainDestroy -= TrainDestroy;
    }

    public void SetPlatformData()
    {
        for (int i = 0; i < platformDataTable.Count; i++)
        {
            if (int.Parse(platformDataTable[i]["LEVEL"].ToString()) == LevelManager.Instance.Level)
            {
                _trainSpeed = float.Parse(platformDataTable[i]["TRAINSPEED"].ToString());
                _platformDistance = float.Parse(platformDataTable[i]["PLATFORMDISTANCE"].ToString());
                break;
            }
        }
        _backGroundController.SetBackGroundSpeed();
    }

    public void OnGameStart()
    {
        OnPlatformRunning?.Invoke();
    }

    void Update()
    {
        if (GameManager.Instance.IsPaused())
        {
            return;
        }
        if (_trainDestroy)
        {
            return;
        }

        if (!GameManager.Instance.IsPaused() && _train.IsRunning)
        {
            _backGroundController.OnUpdate();
        }

        if (GameManager.Instance.IsGamePlaying() && _train.IsRunning)
        {
            if(_platformDistance == 0)
            {
                return;
            }

            _platformDistance -= _trainSpeed * Time.deltaTime;
            OnDistanceChanged?.Invoke(_platformDistance);
            if (_platformDistance <= 0)
            {
                _platformDistance = 0;
                OnDistanceZero?.Invoke();
                return;
            }
        }
    }
    public void TrainDestroy()
    {
        _trainDestroy = true;
    }
    public void Arrived()
    {
        StartCoroutine(ArrivedProcess());
    }


    public IEnumerator ArrivedProcess()
    {
        _train.StopRunning();
        bool isDone = false;
        StartCoroutine(_backGroundController.ArrivedAnimationProcess(() => isDone = true));
        yield return new WaitUntil(()=>isDone);
        OnPlatformArrived?.Invoke();
    }

    public void ResetPlatform()
    {
        SetPlatformData();
        OnReset?.Invoke();
    }
}
