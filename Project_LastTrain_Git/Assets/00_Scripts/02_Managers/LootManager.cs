using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : SingletonManager<LootManager>
{

    int coinCnt;
    public GameObject coinPrefab;
    List<Coin> coinList = new List<Coin>();

    void Start()
    {
        Init();
    }

    public void Init()
    {
        coinCnt = 0;
        GameObject coins = new GameObject("Coins");
        for (int i = 0; i < 150; i++)
        {
            GameObject coin = Instantiate(coinPrefab);
            coin.transform.SetParent(coins.transform);
            coinList.Add(coin.GetComponent<Coin>());
            coinList[i].GetCoin += IncreaseCoin;
            GravityManager.Instance.AddGravityObj(coinList[i].GetComponent<IGravityAffected>());
            GameManager.Instance.OnStageClear += AllCoinUnActive;
            coin.SetActive(false);
        }
    }
    public void OnDisable()
    {
        for (int i = 0; i < coinList.Count; i++)
        {
            coinList[i].GetCoin -= IncreaseCoin;
            Destroy(coinList[i].gameObject);
        }
        GameManager.Instance.OnStageClear -= AllCoinUnActive;

    }


    public void Update()
    {
        for (int i = 0; i < coinList.Count; i++)
        {
            if (!coinList[i].gameObject.activeSelf)
            {
                continue;
            }
            coinList[i].OnUpdate();
        }
    }

    public void DropCoinAt(Enemy enemyInfo)
    {
        for (int i = 0; i < enemyInfo.enemyData.coin; i++)
        {
            for (int j = 0; j < coinList.Count; j++)
            {
                if (coinList[j].gameObject.activeSelf)
                {
                    continue;
                }
                coinList[j].transform.position = new Vector3(enemyInfo.transform.position.x + Random.Range(-2f, 2f), enemyInfo.transform.position.y, 0);
                coinList[j].gameObject.SetActive(true);
                break;
            }
        }
    }

    public void IncreaseCoin()
    {
        coinCnt++;
        Debug.Log($"CoinCnt : {coinCnt}");
    }

    public void DecreaseCoin(int useCoin)
    {
        coinCnt -= useCoin;
    }

    public void AllCoinUnActive()
    {
        for (int i = 0; i < coinList.Count; i++)
        {
            coinList[i].gameObject.SetActive(false);
        }
    }

    public int GetHasCoin()
    {
        return coinCnt;
    }
}
