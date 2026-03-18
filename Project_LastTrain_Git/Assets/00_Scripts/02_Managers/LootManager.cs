using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LootManager : SingletonManager<LootManager>
{
    [SerializeField] List<ItemData> _itemDatas;
    [SerializeField] List<DropTable> _dropList;
    Dictionary<Type, ItemData> _itemDataDics;
    Dictionary<Type, List<Item>> _itemDics;
    Dictionary<Type, int> _itemResourceCntDics;


    List<Item> _activeItemList;
    List<Item> _removeItemList;

    UI_Coin _uiCoin;
    public event Action<int> OnItemCountChanged;
    
    void Awake()
    {
        Init();
    }

    public void Init()
    {
        _itemResourceCntDics = new Dictionary<Type, int>();
        _itemDataDics = new Dictionary<Type, ItemData>();
        _itemDics = new Dictionary<Type, List<Item>>();

        _activeItemList = new List<Item>();
        _removeItemList = new List<Item>();
        for(int i = 0; i < _itemDatas.Count; i++)
        {
            _itemDataDics[_itemDatas[i].GetItemType()] = _itemDatas[i];
        }


        for (int i = 0; i < _dropList.Count; i++)
        {
            List<DropTable.DropEntry> currentDropEntry = _dropList[i].DropItems;
            for(int j = 0; j < currentDropEntry.Count; j++)
            {
                _itemResourceCntDics[currentDropEntry[j].Item.GetType()] = 0;
                GameObject items = new GameObject(currentDropEntry[j].Item.gameObject.name);
                for (int k = 0; k < 50; k++)
                {
                    GameObject item = Instantiate(currentDropEntry[j].Item.gameObject);

                    Item itemScript = item.GetComponent<Item>();
                    item.name = $"{itemScript.GetType()}_{k + 1}";
                    item.transform.SetParent(items.transform);
                    if (!_itemDics.ContainsKey(itemScript.GetType()))
                    {
                        _itemDics[itemScript.GetType()] = new List<Item>();
                    }
                    _itemDics[itemScript.GetType()].Add(itemScript);
                    
                    item.SetActive(false);
                }
            }
        }
        UIManager.Instance.ShowUIAt<UI_Coin>(new Vector3(300,-140,0));
    }
    public void Update()
    {
        for(int i = 0; i < _activeItemList.Count; i++)
        {
            if (!_activeItemList[i].gameObject.activeSelf)
            {
                _removeItemList.Add(_activeItemList[i]);
                continue;
            }
            _activeItemList[i].OnUpdate();
        }
        for(int i = 0; i < _removeItemList.Count; i++)
        {
            _activeItemList.Remove(_removeItemList[i]);
        }
        _removeItemList.Clear();
    }

    public void DropItemAt<T>(T dropTarget) where T : IDroppedItem 
    {
        int rndCnt;
        float rndChance;
        DropTable  currentDropTable = _dropList[0];

        for(int i = 0; i < currentDropTable.DropItems.Count; i++)
        {
            rndCnt = UnityEngine.Random.Range(1, 3);
            rndChance = UnityEngine.Random.Range(0f, 1f);
            if (rndChance <= currentDropTable.DropItems[i].DropChance)
            {
                Type currentItemType = currentDropTable.DropItems[i].Item.GetType();
                for (int j = 0; j < rndCnt; j++)
                {
                    for (int k = 0; k < _itemDics[currentItemType].Count; k++)
                    {

                        if (_itemDics[currentItemType][k].gameObject.activeSelf)
                        {
                            continue;
                        }
                        GameObject itemObject = _itemDics[currentItemType][k].gameObject;
                        Item itemScript = _itemDics[currentItemType][k];
                        itemObject.SetActive(true);
                        itemScript.Init(_itemDataDics[currentItemType], dropTarget.DropPoint);
                        _activeItemList.Add(_itemDics[currentItemType][k]);
                        break;
                    }
                }
            }

        }
    }

    public void IncreaseResource<T>() where T : Item
    {
        Debug.Log("æ∆¿Ã≈€ »πµÊ!");
        _itemResourceCntDics[typeof(T)]++;
        OnItemCountChanged?.Invoke(_itemResourceCntDics[typeof(T)]);
    }
    public void IncreaseResource<T>(int amount) where T : Item
    {
        _itemResourceCntDics[typeof(T)] += amount;
        OnItemCountChanged?.Invoke(_itemResourceCntDics[typeof(T)]);
    }


    public void IncreaseResource(Item item)
    {
        _itemResourceCntDics[item.GetType()]++;
        OnItemCountChanged?.Invoke(_itemResourceCntDics[item.GetType()]);
    }
    public void IncreaseResourceByAmount(Item item, int amount)
    {
        _itemResourceCntDics[item.GetType()] += amount;
    }


    public void DecreaseResource<T>(int useAmount) where T : Item
    {
        _itemResourceCntDics[typeof(T)] -= useAmount;
    }
    public void DecreaseCoin(int useAmount)
    {
        _itemResourceCntDics[typeof(Coin)] -= useAmount;
        OnItemCountChanged?.Invoke(_itemResourceCntDics[typeof(Coin)]);
    }

    public void AllCoinUnActive()
    {
      
    }

    public int GetHasItem<T>() where T : Item
    {
        return _itemResourceCntDics[typeof(T)];
    }

    public int GetHasCoin()
    {
        return GetHasItem<Coin>();
    }
}
