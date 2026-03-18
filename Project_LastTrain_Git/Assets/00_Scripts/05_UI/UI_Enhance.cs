using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Enhance : UI_Base
{
    [SerializeField] Text _playerLevelText;
    [SerializeField] Text _trainHpText;
    [SerializeField] Button _enhanceButton;
    [SerializeField] Button _fixButton;
    
    public void OnEnable()
    {
        _enhanceButton.onClick.AddListener(EnhanceManager.Instance.Enhance);
        _fixButton.onClick.AddListener(EnhanceManager.Instance.FixTrain);
    }

    public void OnDisable()
    {
        _enhanceButton.onClick.RemoveListener(EnhanceManager.Instance.Enhance);
        _fixButton.onClick.RemoveListener(EnhanceManager.Instance.FixTrain);
    }

    public void SetPlayerLevelText(int level)
    {
        Debug.Log(level);
        Debug.Log("플레이어 텍스트 조정 완료!");
        //_playerLevelText.text = $"Level : {level}";
    }

    public void SetTrainHpText(float hp)
    {
        Debug.Log(hp);
        Debug.Log("플레이어 텍스트 조정 완료!");
        //_trainHpText.text = $"Hp : {hp}";
    }

}
