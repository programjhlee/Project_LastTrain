using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Enhance : UI_Base
{
    [SerializeField] Button enhanceButton;
    [SerializeField] Button fixButton;
    

    public void OnEnable()
    {
        enhanceButton.onClick.AddListener(EnhanceManager.Instance.Enhance);
        fixButton.onClick.AddListener(EnhanceManager.Instance.FixTrain);
    }

    public void OnDisable()
    {
        enhanceButton.onClick.RemoveListener(EnhanceManager.Instance.Enhance);
        fixButton.onClick.RemoveListener(EnhanceManager.Instance.FixTrain);
    }

}
