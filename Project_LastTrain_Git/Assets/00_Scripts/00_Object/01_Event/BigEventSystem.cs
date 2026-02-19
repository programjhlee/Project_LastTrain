using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEventSystem : MonoBehaviour
{
    [SerializeField] PlatformController platformController;
    [SerializeField] GameObject bigEventPrefab;
    
    BigEvent bigEvent;
    Train train;
    List<Dictionary<string, object>> bigEventSpawnData;
    
    float rndTime;
    float curTime;
    int currentIdx;
    void Start()
    {
        train = GetComponent<Train>();
        bigEventSpawnData = DataManager.Instance.GetData((int)Define.DataTables.BigEventSpawnData);
        bigEvent = Instantiate(bigEventPrefab).GetComponent<BigEvent>();
        bigEvent.gameObject.SetActive(false);
        GameManager.Instance.OnGameStart += SetBigEventSystem;
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
            bigEvent.gameObject.SetActive(true);
            bigEvent.Init(platformController.TrainSpeed,train);
            rndTime = Random.Range(int.Parse(bigEventSpawnData[currentIdx]["SPAWNINTERVALMIN"].ToString()), int.Parse(bigEventSpawnData[currentIdx]["SPAWNINTERVALMAX"].ToString()));
        }
    }
}
