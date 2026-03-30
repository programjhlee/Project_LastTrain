using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
        GameManager.Instance.OnAllStageClear += () => gameObject.SetActive(false);
        GameManager.Instance.OnGameOver += () => gameObject.SetActive(false);
        train.OnHpChanged += SetTrainHp;
        trainHpSlider.value = 1f;
    }
    public void OnDisable()
    {
        train.OnHpChanged -= SetTrainHp;
    }
    public void SetTrainHp(float ratio)
    {
        trainHpSlider.DOValue(ratio, 0.5f).SetEase(Ease.OutBounce);
    }
}
