using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TrainHP : UI_Base
{
    [SerializeField] Train train;
    [SerializeField] Slider trainHpSlider;

    public void Awake()
    {
        train = GameObject.FindAnyObjectByType<Train>();
        trainHpSlider = transform.GetComponentInChildren<Slider>();
    }

    public void OnEnable()
    {
        GameManager.Instance.OnGameStart += SetTrainHp;
        train.OnDamaged += SetTrainHp;
        train.OnFixed += SetTrainHp;
    }
    public void OnDisable()
    {
        GameManager.Instance.OnGameStart -= SetTrainHp;
        train.OnDamaged -= SetTrainHp;
        train.OnFixed -= SetTrainHp;
    }
    public void SetTrainHp()
    {
        trainHpSlider.value = train.CurHp / train.MaxHp;
    }
}
