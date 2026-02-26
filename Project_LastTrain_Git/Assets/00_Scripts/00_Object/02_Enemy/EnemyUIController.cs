using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyUIController : MonoBehaviour
{

    Dictionary<Type, UI_HUD> _enemyUIDics = new Dictionary<Type, UI_HUD>();

    int _hudlayer = 1;

    public void AddUI<T>(UI_HUD uiHUD) where T : UI_HUD
    {
        _enemyUIDics[typeof(T)] = uiHUD;
        uiHUD.Bind(transform, _hudlayer);
        _hudlayer++;
    }
    public void UpdateUIPos()
    {
        foreach(var _enemyUI in _enemyUIDics.Values)
        {
            _enemyUI.UpdatePos();
        }
    }

    public T GetUIHUD<T>(string name = null) where T : UI_HUD
    {
        Type type = typeof(T);
        return _enemyUIDics[type] as T;
    }

    public void AllUIShow()
    {
        foreach (var _enemyUI in _enemyUIDics.Values)
        {
            _enemyUI.Show();
        }
    }
    public void AllUIHIde()
    {
        foreach (var _enemyUI in _enemyUIDics.Values)
        {
            _enemyUI.Hide();
        }
    }

}
