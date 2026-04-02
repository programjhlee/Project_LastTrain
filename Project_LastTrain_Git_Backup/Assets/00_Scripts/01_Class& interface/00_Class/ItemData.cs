using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ItemData",menuName = "Create ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] int _itemID;
    [SerializeField] string _itemName;

    public int ItemID => _itemID;
    public string ItemName => _itemName;
    public virtual Type GetItemType() => typeof(Item);
}
