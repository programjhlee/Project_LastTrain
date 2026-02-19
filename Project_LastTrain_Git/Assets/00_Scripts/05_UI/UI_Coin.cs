using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Coin : UI_Base
{

    [SerializeField] Text coinText;
    public void SetCoinText(int coin)
    {
        coinText.text = $":{coin.ToString()}";
    }

}
