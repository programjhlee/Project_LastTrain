using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatformController : MonoBehaviour
{
    [SerializeField] Train train;
    [SerializeField] List<GameObject> _rails;
    [SerializeField] GameObject _railPrefab;
    List<Dictionary<string, object>> platformDataTable;
    UI_Distance ui_Distance;
    float trainSpeed;
    float platformDistance;
    bool trainDestroy;
    float _railSizeX;
    float _railStartPosX = -120f;
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
        _rails = new List<GameObject>();
        for(int i = 0; i < 6; i++)
        {
            GameObject rail = Instantiate(_railPrefab);
            rail.transform.localScale = new Vector3(7.5f, 7.5f, 7.5f);
            rail.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            _railSizeX = rail.GetComponentInChildren<Renderer>().bounds.extents.x * 2;
            rail.transform.position = new Vector3(_railStartPosX + i * _railSizeX, -10.1f,0);
            _rails.Add(rail);
        }
        platformDataTable = DataManager.Instance.GetData((int)Define.DataTables.PlatformData);
        ui_Distance = UIManager.Instance.ShowUIAt<UI_Distance>(new Vector3(0,285));
        ui_Distance.Hide();
    }

    void OnEnable()
    {
        SetPlatformData();
        GameManager.Instance.OnGameStart += OnGameStart;
        train.OnTrainDestroy += TrainDestroy;
        trainDestroy = false;
    }
    void OnDisable()
    {
        GameManager.Instance.OnGameStart -= OnGameStart;
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

    public void OnGameStart()
    {
        ui_Distance.Show();
        SetPlatformData();
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

        if (!GameManager.Instance.IsPaused())
        {
            for (int i = 0; i < _rails.Count; i++)
            {
                if (_rails[i].transform.position.x < -_railSizeX * 3)
                {
                    _rails[i].transform.position = _rails[_rails.Count - 1].transform.position + new Vector3(_railSizeX, 0, 0);
                    _rails.Add(_rails[i]);
                    _rails.RemoveAt(i);

                }
                _rails[i].transform.position += Vector3.left * trainSpeed * 50 * Time.deltaTime;
            }
        }

        if (GameManager.Instance.IsGamePlaying())
        {
            platformDistance -= trainSpeed * Time.deltaTime;
            ui_Distance.SetDistanceText(platformDistance);
            if (platformDistance <= 0)
            {
                platformDistance = 0;
                ui_Distance.Hide();
                UIManager.Instance.ShowUI<UI_Enhance>();
                OnArrived?.Invoke();
            }
        }
    }

    public void TrainDestroy()
    {
        trainDestroy = true;
    }
}
