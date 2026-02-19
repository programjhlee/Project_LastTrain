using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatformController : MonoBehaviour
{
    [SerializeField] Train train;
    List<Dictionary<string, object>> platformDataTable;
    float trainSpeed;
    float platformDistance;
    bool trainDestroy;


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

    public event Action OnArrived;

    void Awake()
    {
        platformDataTable = DataManager.Instance.GetData((int)Define.DataTables.PlatformData);
    }

    void OnEnable()
    {
        GameManager.Instance.OnGameStart += SetPlatformData;
        train.OnTrainDestroy += TrainDestroy;
        trainDestroy = false;
    }
    void OnDisable()
    {
        GameManager.Instance.OnGameStart -= SetPlatformData;
        train.OnTrainDestroy -= TrainDestroy;
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
    }

    void Update()
    {
        if (GameManager.Instance.IsGamePlaying() == false)
        {
            return;
        }
        if (trainDestroy)
        {
            return;
        }
        platformDistance -= trainSpeed * Time.deltaTime;
        if (platformDistance <= 0)
        { 
            platformDistance = 0;
            UIManager.Instance.ShowUI<UI_Enhance>();
            OnArrived?.Invoke();
        
        }
    }

    public void TrainDestroy()
    {
        trainDestroy = true;
    }
}
