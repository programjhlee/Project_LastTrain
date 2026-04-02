using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Item : MonoBehaviour
{
    protected ItemData _itemData;

    public event Action<Item> GetItem;
    public virtual void Init(ItemData itemData,Vector3 spawnPos)
    {
        _itemData = itemData;
        transform.position = spawnPos;
    }
    public abstract void OnUpdate();

    public abstract void Clear();
    public virtual void InvokeGetItem(Item item)
    {
        GetItem?.Invoke(item);
    }
    public virtual void ReleaseEvent()
    {
        GetItem = null;
    }
}
