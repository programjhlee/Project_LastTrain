using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Coin : UI_Base
{

    [SerializeField] Text coinText;

    public void Awake()
    {
        LootManager.Instance.OnItemCountChanged += SetCoinText;
    }
    public void SetCoinText(int coin)
    {
        Debug.Log("UI°»½Å!");
        coinText.text = $" : {coin.ToString()}";
    }

}
