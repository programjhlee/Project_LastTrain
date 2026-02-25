using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : SingletonManager<UIManager>
{

    [SerializeField] Canvas canvasHUD;
    [SerializeField] Canvas canvasWorld;

    [SerializeField] List<UI_Base> _uiPrefabs = new List<UI_Base>();
    [SerializeField] List<UI_World> _uiWorldPrefabs = new List<UI_World>();
    
    Dictionary<Type, List<UI_Base>> _uiDics = new Dictionary<Type,List<UI_Base>>();
    Dictionary<Type, List<UI_World>> _uiWorldDics = new Dictionary<Type,List<UI_World>>();
    
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
        for (int i = 0; i < _uiWorldPrefabs.Count; i++)
        {
            Type type = _uiWorldPrefabs[i].GetType();
            if (!_uiDics.ContainsKey(type))
            {
                _uiWorldDics[type] = new List<UI_World>();
                _uiWorldDics[type].Add(_uiWorldPrefabs[i]);
            }
            _uiWorldDics[type].Add(_uiWorldPrefabs[i]);
        }
    }

    public T ShowUI<T>(string name = null) where T : UI_Base
    {
        Type type = typeof(T);
        T ui = null;
        T[] uiInCanvas = canvasHUD.GetComponentsInChildren<T>(true);
        
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
                        ui.transform.SetParent(canvasHUD.transform);
                        ui.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                        ui.Show();
                        break;
                    }
                    if (uis[i].name == name)
                    {
                        ui = Instantiate(uis[i]).GetComponent<T>();
                        ui.name = name;
                        ui.transform.SetParent(canvasHUD.transform);
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

    public T ShowUIWorld<T>(Transform target, string name = null) where T : UI_World
    {
        Type type = typeof(T);
        T ui = null;
        T[] uiInCanvas = canvasWorld.GetComponentsInChildren<T>(true);

        for (int i = 0; i < uiInCanvas.Length; i++)
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

        if (ui == null)
        {
            if (_uiWorldDics.TryGetValue(type, out List<UI_World> uis))
            {
                for (int i = 0; i < uis.Count; i++)
                {
                    if (name == null)
                    {
                        Debug.Log("Ã£¾Ò´Ù!");
                        ui = Instantiate(uis[0]).GetComponent<T>();
                        ui.name = typeof(T).Name;
                        ui.transform.SetParent(canvasWorld.transform);
                        ui.Bind(target);
                        ui.Show();
                        break;
                    }
                    if (uis[i].name == name)
                    {
                        ui = Instantiate(uis[i]).GetComponent<T>();
                        ui.name = name;
                        ui.transform.SetParent(canvasWorld.transform);
                        ui.Bind(target);
                        ui.Show();
                        break;
                    }
                }
            }
        }
        return ui;
    }
    public void HideUIWorld<T>() where T : UI_World
    {
        Type type = typeof(T);
        T ui = null;
        foreach (var uiInCanvas in canvasWorld.transform.GetComponentsInChildren<T>(true))
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
