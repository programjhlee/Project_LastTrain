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
        train.OnHpChanged += SetTrainHp;
    }
    public void OnDisable()
    {
        train.OnHpChanged += SetTrainHp;
    }
    public void SetTrainHp(float ratio)
    {
        trainHpSlider.value = ratio;
    }
}
