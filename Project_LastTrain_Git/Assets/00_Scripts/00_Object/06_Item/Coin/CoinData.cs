using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CoinData", menuName ="Create ItemData/CoinData")]
public class CoinData : ItemData
{
    [SerializeField] int _price;
    public int Price => _price;
    public override Type GetItemType() => typeof(Coin);
   
}
