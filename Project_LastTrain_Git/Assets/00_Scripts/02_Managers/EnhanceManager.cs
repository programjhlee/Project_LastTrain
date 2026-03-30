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
    UI_Enhance _ui_Enhance;

    List<Dictionary<string, object>> _enhancePriceData;
    List<Dictionary<string, object>> _enhanceValueData;

    public event Action OnPlayerEnhanceSucess;
    public event Action OnFixSucess;
    public event Action OnNotEnoughMoney;
    public event Action OnTrainHpFull;
    public event Action<PlayerData> OnPlayerEnhance;


    int enhancePrice;
    int fixPrice;

    public int EnhancePrice
    {
        get
        {
            return enhancePrice;
        }
    }

    public int FixPrice
    {
        get { return fixPrice; }
    }


    public void Awake()
    {
        Init();
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
    }

    public void HideEnhanceUI()
    {
        if(_ui_Enhance != null)
        {
            _ui_Enhance.Hide();
        }
    }

    public void FixTrain()
    {
        if(LootManager.Instance.GetHasCoin() < fixPrice)
        {
            OnNotEnoughMoney?.Invoke();
            return;
        }
        if(_train.CurHp >= _train.MaxHp)
        {
            OnTrainHpFull?.Invoke();
            return;
        }
        LootManager.Instance.DecreaseCoin(fixPrice);
        OnFixSucess?.Invoke();
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
            OnNotEnoughMoney?.Invoke();
            return;
        };
        LootManager.Instance.DecreaseCoin(enhancePrice);
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
        OnPlayerEnhanceSucess?.Invoke();
    }
}
