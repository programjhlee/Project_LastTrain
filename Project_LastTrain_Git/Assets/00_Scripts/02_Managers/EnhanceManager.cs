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

    [SerializeField] PlayerData _playerData;
    [SerializeField] Train _train;
    [SerializeField] PlatformController _platformController;
    [SerializeField] AudioClip _fixSound;
    [SerializeField] AudioClip _enhanceSound;
    UI_Enhance _ui_Enhance;

    List<Dictionary<string, object>> _enhancePriceData;
    List<Dictionary<string, object>> _enhanceValueData;

    public event Action<PlayerData> OnPlayerEnhance;
    public event Action<float> OnTrainHpChanged;

    int enhancePrice;
    int fixPrice;

    public void Awake()
    {
        Init();
    }
    public void Start()
    {
    }
    public void Init()
    {
        _enhancePriceData = DataManager.Instance.GetData((int)Define.DataTables.EnhancePriceData);
        _enhanceValueData = DataManager.Instance.GetData((int)Define.DataTables.EnhanceValueData);

        _platformController.OnPlatformArrived += ShowEnhanceUI;

        for (int i = 0; i < _enhancePriceData.Count; i++)
        {
            if (_enhancePriceData[i]["ENHANCETYPE"].ToString() == "PLAYERENHANCE")
            {
                enhancePrice = int.Parse(_enhancePriceData[i]["PRICE"].ToString());
            }
            if (_enhancePriceData[i]["ENHANCETYPE"].ToString() == "FIXTRAIN")
            {
                fixPrice = int.Parse(_enhancePriceData[i]["PRICE"].ToString());
            }
        }
    }

    public void ShowEnhanceUI()
    {
        _ui_Enhance = UIManager.Instance.ShowUI<UI_Enhance>();
        _ui_Enhance.transform.SetAsFirstSibling();
        _ui_Enhance.SetPlayerDataText(_playerData);
    }


    public void FixTrain()
    {
        if(LootManager.Instance.GetHasCoin() < fixPrice)
        {
            Debug.Log("돈부족함..");
            return;
        }
        if(_train.CurHp >= _train.MaxHp)
        {
            Debug.Log("열차 체력 만땅..");
            return;
        }
        Debug.Log("열차 수리 완료!");
        LootManager.Instance.DecreaseCoin(fixPrice);
        SoundManager.Instance.PlaySFX(_fixSound);
        for (int i = 0; i < _enhanceValueData.Count; i++)
        {
            if (_enhanceValueData[i]["ENHANCETYPE"].ToString() == "FIXTRAIN")
            {
                _train.TakeFix(int.Parse(_enhanceValueData[i]["VALUE"].ToString()));
                break;
            }
        }
    }

    public void Enhance()
    {
        if (LootManager.Instance.GetHasCoin() < enhancePrice)
        {
            return;
        };
        LootManager.Instance.DecreaseCoin(enhancePrice);
        SoundManager.Instance.PlaySFX(_enhanceSound);
        _playerData.Level++;
        string[] enhanceTypes = Enum.GetNames(typeof(EnhanceTypes));
        for(int i = 0; i < enhanceTypes.Length; i++)
        {
            string enhanceType = enhanceTypes[i];
            for(int j = 0; j < _enhanceValueData.Count; j++)
            {
                if (_enhanceValueData[j]["STATTYPE"].ToString() == enhanceType)
                {
                    switch (enhanceType)
                    {
                        case "MoveSpeed":
                            _playerData.MoveSpeed += float.Parse(_enhanceValueData[j]["VALUE"].ToString());
                            break;
                        case "AttackPower":
                            _playerData.AttackPower += float.Parse(_enhanceValueData[j]["VALUE"].ToString());
                            break;
                        case "FixPower":
                            _playerData.FixPower += float.Parse(_enhanceValueData[j]["VALUE"].ToString());
                            break;
                    }
                }
            }
        }
        OnPlayerEnhance?.Invoke(_playerData);
    }
}
