using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyUIController : MonoBehaviour
{
    [SerializeField] UI_HUDValueBarStrategyData _enemyHUDGraphicData;
    Enemy _enemy;
    IAttackable _attackableEnemy;
    UI_HUDValueBar _enemyHUDValueBar;

    public void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _attackableEnemy = GetComponent<IAttackable>();
    }
    public void Init()
    {
        _enemyHUDValueBar = UIManager.Instance.ShowUIHUD<UI_HUDValueBar>(transform);
        _enemyHUDValueBar.Init(_enemyHUDGraphicData);
        _enemyHUDValueBar.SetValue(_attackableEnemy.Curhp / _attackableEnemy.Maxhp);
    }

    public void SetValueBarRatio()
    {
        _enemyHUDValueBar.SetValue(_attackableEnemy.Curhp / _attackableEnemy.Maxhp);
    }
    
    public void UpdateUIPos()
    {
        _enemyHUDValueBar.UpdatePos();
    }

    public void HideUIHUD()
    {
        _enemyHUDValueBar.Hide();
    }

}
