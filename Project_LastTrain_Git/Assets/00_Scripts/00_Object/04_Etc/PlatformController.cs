using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatformController : MonoBehaviour
{
    [SerializeField] Train _train;
    TrainSound _trainSound;
    BackGroundController _backGroundController;
    List<Dictionary<string, object>> platformDataTable;
    UI_Distance ui_Distance;
    float trainSpeed;
    float platformDistance;
    bool trainDestroy;

    public float PlatformDistance
    {
        get
        {
            return platformDistance;
        }
        private set
        {
            platformDistance = value;
        }
    }
    public float TrainSpeed
    {
        get
        {
            return trainSpeed;
        }
        private set
        {
            trainSpeed = value;
        }
    }
    public event Action OnDistanceZero;
    public event Action OnPlatformArrived;

    void Awake()
    {
        platformDataTable = DataManager.Instance.GetData((int)Define.DataTables.PlatformData);
        _trainSound = _train.GetComponent<TrainSound>();
        ui_Distance = UIManager.Instance.ShowUIAt<UI_Distance>(new Vector3(0, 285));
        ui_Distance.Hide();

        _backGroundController = GetComponent<BackGroundController>();
        _trainSound = GetComponent<TrainSound>();
        _backGroundController.Init();
    }

    void OnEnable()
    {
        SetPlatformData();
        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnStageClear += Arrived;
        LevelManager.Instance.OnLevelChanged += SetPlatformData;
        _train.OnTrainDestroy += TrainDestroy;
        trainDestroy = false;
    }
    void OnDisable()
    {
        GameManager.Instance.OnGameStart -= OnGameStart;
        GameManager.Instance.OnStageClear -= Arrived;
        LevelManager.Instance.OnLevelChanged -= SetPlatformData;
        _train.OnTrainDestroy -= TrainDestroy;
        OnDistanceZero = null;
    }


    public void SetPlatformData()
    {
        for (int i = 0; i < platformDataTable.Count; i++)
        {
            if (int.Parse(platformDataTable[i]["LEVEL"].ToString()) == LevelManager.Instance.Level)
            {
                trainSpeed = float.Parse(platformDataTable[i]["TRAINSPEED"].ToString());
                platformDistance = float.Parse(platformDataTable[i]["PLATFORMDISTANCE"].ToString());
                break;
            }
        }
        _backGroundController.SpeedInit();
    }

    public void OnGameStart()
    {
        ui_Distance.Show();
    }

    public void OnStageStart()
    {
        ui_Distance.Show();
        _train.StartRunning();
    }


    void Update()
    {
        
        if (GameManager.Instance.IsPaused())
        {
            return;
        }
        if (trainDestroy)
        {
            return;
        }

        if (!GameManager.Instance.IsPaused() && _train.IsRunning)
        {
            _backGroundController.OnUpdate();
        }

        if (GameManager.Instance.IsGamePlaying() && _train.IsRunning)
        {
            if(platformDistance == 0)
            {
                return;
            }

            platformDistance -= trainSpeed * Time.deltaTime;
            ui_Distance.SetDistanceText(platformDistance);
            if (platformDistance <= 0)
            {
                platformDistance = 0;
                ui_Distance.Hide();
                OnDistanceZero?.Invoke();
                return;
            }
        }
    }


    public void TrainDestroy()
    {
        trainDestroy = true;
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
}
