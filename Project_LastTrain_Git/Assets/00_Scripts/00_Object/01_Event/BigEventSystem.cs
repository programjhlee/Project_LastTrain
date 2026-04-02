using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BigEventSystem : MonoBehaviour
{
    [SerializeField] AnnounceStrategyAlert _announceAlert;
    [SerializeField] UI_Announce _uiAnnounce;
    [SerializeField] AudioClip _warningSound;
    [SerializeField] GameObject _bigEventPrefab;
    [SerializeField] Switch _switch;
    [SerializeField] PlatformController _platformController;
    [SerializeField] Train _train;
    
    List<Dictionary<string, object>> _bigEventSpawnData;
    BigEvent _bigEvent;

    float _rndTime;
    float _curTime;
    int _currentIdx;
    public void Awake()
    {
        _bigEventSpawnData = DataManager.Instance.GetData(DataManager.DataTables.BigEventSpawnData);
        _bigEvent = Instantiate(_bigEventPrefab).GetComponent<BigEvent>();
        _bigEvent.gameObject.SetActive(false);
    }
    public void OnEnable()
    {
        LevelManager.Instance.OnLevelChanged += SetBigEventSystem;
        GameManager.Instance.OnStageClear += TurnOffBigEvent;
        GameManager.Instance.OnAllStageClear += TurnOffBigEvent;
        _train.OnTrainDestroy += TurnOffBigEvent;
        _train.OnReset += ResetBigEventSystem;
    }

    public void OnDisable()
    {
        LevelManager.Instance.OnLevelChanged -= SetBigEventSystem;
        GameManager.Instance.OnStageClear -= TurnOffBigEvent;
        GameManager.Instance.OnAllStageClear -= TurnOffBigEvent;
        _train.OnTrainDestroy -= TurnOffBigEvent;
        _train.OnReset -= ResetBigEventSystem;
    }

    public void SetBigEventSystem()
    {
        _curTime = 0;
        for(int i = 0; i < _bigEventSpawnData.Count; i++)
        {
            if (int.Parse(_bigEventSpawnData[i]["LEVEL"].ToString()) == LevelManager.Instance.Level)
            {
                _currentIdx = i;
                _rndTime = UnityEngine.Random.Range(int.Parse(_bigEventSpawnData[i]["SPAWNINTERVALMIN"].ToString()), int.Parse(_bigEventSpawnData[i]["SPAWNINTERVALMAX"].ToString()));
                break;
            }
        }
    }
    void Update()
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }
        _curTime += Time.deltaTime;

        if(_curTime >= _rndTime && !_bigEvent.gameObject.activeSelf)
        {
            _curTime = 0;
            SpawnBigEvent();
        }
    }

    public BigEvent SpawnBigEvent()
    {
        SoundManager.Instance.PlaySFX(_warningSound);
        _switch.SwitchActive();
        _bigEvent.Init(_platformController.TrainSpeed, _train);
        _bigEvent.OnTrainCrashed += _train.TakeDamage;
        _bigEvent.OnDestroy += _switch.SwitchUnActive;
        _bigEvent.OnDestroy += HideAnnounceUI;
        _uiAnnounce = UIManager.Instance.ShowAnnounce(_announceAlert, "GO TO THE FRONT!!", new Vector2(0, 300f));
        _rndTime = UnityEngine.Random.Range(int.Parse(_bigEventSpawnData[_currentIdx]["SPAWNINTERVALMIN"].ToString()), int.Parse(_bigEventSpawnData[_currentIdx]["SPAWNINTERVALMAX"].ToString()));
        return _bigEvent;
    }

    public void HideAnnounceUI()
    {
        if (_uiAnnounce != null)
        {
            _uiAnnounce.Hide();
        }
    }
    public void TurnOffBigEvent()
    {
        _curTime = 0;
        if(_uiAnnounce != null)
        {
            _uiAnnounce.Hide();
        }
        if (_bigEvent.gameObject.activeSelf)
        {
            _switch.SwitchUnActive();
            _bigEvent.gameObject.SetActive(false);
        }
    }

    public void ResetBigEventSystem()
    {
        TurnOffBigEvent();
        SetBigEventSystem();
    }
}
