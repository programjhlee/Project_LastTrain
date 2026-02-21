using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatformController : MonoBehaviour
{
    [SerializeField] Train train;
    List<Dictionary<string, object>> platformDataTable;
    UI_Distance ui_Distance;
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
        ui_Distance = UIManager.Instance.ShowUIAt<UI_Distance>(new Vector3(0,285));
        ui_Distance.Hide();
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
        ui_Distance.Show();
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
        ui_Distance.SetDistanceText(platformDistance);
        if (platformDistance <= 0)
        { 
            platformDistance = 0;
            UIManager.Instance.ShowUI<UI_Enhance>();
            ui_Distance.Hide();
            OnArrived?.Invoke();
        }
    }

    public void TrainDestroy()
    {
        trainDestroy = true;
    }
}
