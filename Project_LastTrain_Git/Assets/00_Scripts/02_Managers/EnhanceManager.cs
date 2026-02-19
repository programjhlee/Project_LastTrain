using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnhanceManager : SingletonManager<EnhanceManager>
{

    enum EnhanceTypes
    {
        MoveSpeed,
        AttackPower,
        FixPower,
    }

    [SerializeField] PlayerData playerData;
    [SerializeField] Train train;
    List<Dictionary<string, object>> enhancePriceData;
    List<Dictionary<string, object>> enhanceValueData;
    
    int enhancePrice;
    int fixPrice;

    public void Awake()
    {
        Init();
    }
    public void Init()
    {
        enhancePriceData = DataManager.Instance.GetData((int)Define.DataTables.EnhancePriceData);
        enhanceValueData = DataManager.Instance.GetData((int)Define.DataTables.EnhanceValueData);
        for (int i = 0; i < enhancePriceData.Count; i++)
        {
            if (enhancePriceData[i]["ENHANCETYPE"].ToString() == "PLAYERENHANCE")
            {
                enhancePrice = int.Parse(enhancePriceData[i]["PRICE"].ToString());
            }
            if (enhancePriceData[i]["ENHANCETYPE"].ToString() == "FIXTRAIN")
            {
                fixPrice = int.Parse(enhancePriceData[i]["PRICE"].ToString());
            }
        }
    }


    public void FixTrain()
    {
        if(LootManager.Instance.GetHasCoin() < fixPrice)
        {
            Debug.Log("돈부족함..");
            return;
        }
        if(train.CurHp >= train.MaxHp)
        {
            Debug.Log("열차 체력 만땅..");
            return;
        }
        Debug.Log("열차 수리 완료!");
        LootManager.Instance.DecreaseCoin(fixPrice);

        for(int i = 0; i < enhanceValueData.Count; i++)
        {
            if (enhanceValueData[i]["ENHANCETYPE"].ToString() == "FIXTRAIN")
            {
                train.TakeFix(int.Parse(enhanceValueData[i]["VALUE"].ToString()));
                break;
            }
        }
    }

    public void Enhance()
    {
        if (LootManager.Instance.GetHasCoin() < enhancePrice)
        {
            Debug.Log("돈부족함..");
            return;
        };
        Debug.Log("강화 성공!");
        LootManager.Instance.DecreaseCoin(enhancePrice);
        string[] enhanceTypes = Enum.GetNames(typeof(EnhanceTypes));
        for(int i = 0; i < enhanceTypes.Length; i++)
        {
            string enhanceType = enhanceTypes[i];
            for(int j = 0; j < enhanceValueData.Count; j++)
            {
                if (enhanceValueData[j]["STATTYPE"].ToString() == enhanceType)
                {
                    switch (enhanceType)
                    {
                        case "MoveSpeed":
                            playerData.MoveSpeed += float.Parse(enhanceValueData[j]["VALUE"].ToString());
                            break;
                        case "AttackPower":
                            playerData.AttackPower += float.Parse(enhanceValueData[j]["VALUE"].ToString());
                            break;
                        case "FixPower":
                            playerData.FixPower += float.Parse(enhanceValueData[j]["VALUE"].ToString());
                            break;
                    }
                }
            }
        }
    }
}
