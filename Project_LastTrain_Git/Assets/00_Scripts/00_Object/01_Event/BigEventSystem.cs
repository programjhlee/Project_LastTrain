using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEventSystem : MonoBehaviour
{
    [SerializeField] GameObject _bigEventPrefab;
    [SerializeField] Switch _switch;
    [SerializeField] PlatformController _platformController;
    [SerializeField] Train train;
    
    List<Dictionary<string, object>> bigEventSpawnData;
    BigEvent bigEvent;

    float rndTime;
    float curTime;
    int currentIdx;
    void Start()
    {
        bigEventSpawnData = DataManager.Instance.GetData((int)Define.DataTables.BigEventSpawnData);
        bigEvent = Instantiate(_bigEventPrefab).GetComponent<BigEvent>();
        bigEvent.gameObject.SetActive(false);
        GameManager.Instance.OnGameStart += SetBigEventSystem;
        GameManager.Instance.OnStageClear += TurnOffBigEvent;
    }

    public void SetBigEventSystem()
    {
        curTime = 0;
        for(int i = 0; i < bigEventSpawnData.Count; i++)
        {
            if (int.Parse(bigEventSpawnData[i]["LEVEL"].ToString()) == LevelManager.Instance.Level)
            {
                currentIdx = i;
                rndTime = Random.Range(int.Parse(bigEventSpawnData[i]["SPAWNINTERVALMIN"].ToString()), int.Parse(bigEventSpawnData[i]["SPAWNINTERVALMAX"].ToString()));
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }
       
        curTime += Time.deltaTime;

        if(curTime >= rndTime && !bigEvent.gameObject.activeSelf)
        {
            curTime = 0;
            _switch.SwitchActive();

            bigEvent.gameObject.SetActive(true);
            bigEvent.OnTrainCrashed += train.TakeDamage;
            bigEvent.OnDestroy += _switch.SwitchUnActive;
            bigEvent.Init(_platformController.TrainSpeed,train);
            
            rndTime = Random.Range(int.Parse(bigEventSpawnData[currentIdx]["SPAWNINTERVALMIN"].ToString()), int.Parse(bigEventSpawnData[currentIdx]["SPAWNINTERVALMAX"].ToString()));
        }
    }

    public void TurnOffBigEvent()
    {
        if (bigEvent.gameObject.activeSelf)
        {
            bigEvent.gameObject.SetActive(false);
        }
    }
}
