using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyUIController : MonoBehaviour
{
    Enemy _enemy;
    UI_HUDValueBar _enemyHUDValueBar;
    public void Init()
    {
        _enemy = GetComponent<Enemy>();
        _enemyHUDValueBar = UIManager.Instance.ShowUIHUD<UI_HUDValueBar>(transform);
        _enemy.OnDamage += SetValueBarRatio;
    }

    public void SetValueBarRatio(EnemyData enemyData)
    {
        _enemyHUDValueBar.SetValue(enemyData.curHp / enemyData.maxHp);
    }
    
}
