using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : SingletonManager<UIManager>
{

    [SerializeField] Canvas canvas;
    [SerializeField] Canvas canvasHUD;

    [SerializeField] List<UI_Base> _uiPrefabs = new List<UI_Base>();
    [SerializeField] List<UI_HUD> _uiHUDPrefabs = new List<UI_HUD>();
    
    Dictionary<Type, List<UI_Base>> _uiDics = new Dictionary<Type,List<UI_Base>>();
    Dictionary<Type, List<UI_HUD>> _uiHUDDics = new Dictionary<Type,List<UI_HUD>>();
    
    public void Awake()
    {
        Init();
    }


    public void Init()
    {
        for (int i = 0; i < _uiPrefabs.Count; i++)
        {
            Type type = _uiPrefabs[i].GetType();
            if (!_uiDics.ContainsKey(type))
            {
                _uiDics[type] = new List<UI_Base>();
                _uiDics[type].Add(_uiPrefabs[i]);
            }
            _uiDics[type].Add(_uiPrefabs[i]);
        }
        for (int i = 0; i < _uiHUDPrefabs.Count; i++)
        {
            Type type = _uiHUDPrefabs[i].GetType();
            if (!_uiDics.ContainsKey(type))
            {
                _uiHUDDics[type] = new List<UI_HUD>();
                _uiHUDDics[type].Add(_uiHUDPrefabs[i]);
            }
            _uiHUDDics[type].Add(_uiHUDPrefabs[i]);
        }
    }

    public T ShowUI<T>(string name = null) where T : UI_Base
    {
        Type type = typeof(T);
        T ui = null;
        T[] uiInCanvas = canvas.GetComponentsInChildren<T>(true);
        
        for(int i = 0; i < uiInCanvas.Length; i++)
        {
            if (name == null)
            {
                ui = uiInCanvas[0];
                ui.Show();
            }
            else if (uiInCanvas[i].name == name)
            {
                ui = uiInCanvas[i];
                ui.Show();
            }
        }

        if(ui == null)
        {
            if(_uiDics.TryGetValue(type,out List<UI_Base> uis))
            {
                for(int i = 0; i < uis.Count; i++)
                {
                    if(name == null)
                    {
                        ui = Instantiate(uis[0]).GetComponent<T>();
                        ui.name = typeof(T).Name;
                        ui.transform.SetParent(canvas.transform);
                        ui.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                        ui.Show();
                        break;
                    }
                    if (uis[i].name == name)
                    {
                        ui = Instantiate(uis[i]).GetComponent<T>();
                        ui.name = name;
                        ui.transform.SetParent(canvas.transform);
                        ui.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                        ui.Show();
                        break;
                    }
                }
            }
        }
        return ui;
    }

    public T ShowUIAt<T>(Vector2 pos,string name = null) where T : UI_Base
    {
        T ui = ShowUI<T>(name);

        if (ui != null)
        {
            ui.GetComponent<RectTransform>().anchoredPosition = pos;
        }
        return ui;
    }

    public void HideUI<T>() where T : UI_Base
    {
        Type type = typeof(T);
        T ui = null;
        foreach (var uiInCanvas in canvas.transform.GetComponentsInChildren<T>(true))
        {
            if (uiInCanvas.GetType() == type)
            {
                ui = uiInCanvas;
                ui.Hide();
            }
            else
            {
                Debug.Log("Cant Find UI");
            }
        }
    }

    public T ShowUIHUD<T>(Transform target, float upDirScale = 1, string uiname = null) where T : UI_HUD
    {
        Type type = typeof(T);
        T ui = null;
        T[] uiInCanvas = canvasHUD.GetComponentsInChildren<T>(true);

        if (_uiHUDDics.TryGetValue(type, out List<UI_HUD> uis))
        {
            for (int i = 0; i < uis.Count; i++)
            {
                if (uiname == null)
                {
                    ui = Instantiate(uis[0]).GetComponent<T>();
                    ui.name = typeof(T).Name;
                    ui.transform.SetParent(canvasHUD.transform);
                    ui.Bind(target, upDirScale);
                    ui.Show();
                    break;
                }
                if (uis[i].name == uiname)
                {
                    ui = Instantiate(uis[i]).GetComponent<T>();
                    ui.name = name;
                    ui.transform.SetParent(canvasHUD.transform);
                    ui.Bind(target, upDirScale);
                    ui.Show();
                    break;
                }
            }
        }
        return ui;
    }
    public void HideUIHUD<T>() where T : UI_HUD
    {
        Type type = typeof(T);
        T ui = null;
        foreach (var uiInCanvas in canvasHUD.transform.GetComponentsInChildren<T>(true))
        {
            if (uiInCanvas.GetType() == type)
            {
                ui = uiInCanvas;
                ui.Hide();
            }
            else
            {
                Debug.Log("Cant Find UI");
            }
        }
    }
}
