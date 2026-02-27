using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIHUDController : MonoBehaviour
{
    Dictionary<Type, List<UI_HUD>> _uiDics;


    public void Init()
    {
        _uiDics = new Dictionary<Type, List<UI_HUD>>();
    }


    public void AddUIHUD(UI_HUD uiHUD)
    {
       Type key = uiHUD.GetType();
        if (_uiDics[key] == null)
        {
            _uiDics[key] = new List<UI_HUD>();
        }
        _uiDics[key].Add(uiHUD);
    }

    public void UpdateUIHUDPos()
    {
        foreach (var key in _uiDics.Keys)
        {
            foreach(var hud in _uiDics[key])
            {
                hud.UpdatePos();
            }
        }
    }

    public void UIHUDListClear()
    {
        foreach (var key in _uiDics.Keys)
        {
            foreach (var hud in _uiDics[key])
            {
                hud.Hide();
            }
            _uiDics[key].Clear();
        }
    }

    public T GetHUDUI<T>(string name = null) where T : UI_HUD
    {
        T returnUI = null;
        List<UI_HUD> ui;
        if (_uiDics.TryGetValue(typeof(T),out ui))
        {
            if (name == null)
            {
                foreach (var hud in ui)
                {
                    if (typeof(T) == hud.GetType())
                    {
                        returnUI = hud as T;
                    }
                }
            }
        }
        return returnUI;
    }

}
