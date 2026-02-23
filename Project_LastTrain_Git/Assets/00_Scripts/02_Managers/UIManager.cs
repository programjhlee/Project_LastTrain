using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : SingletonManager<UIManager>
{

    [SerializeField] Canvas canvas;
    [SerializeField] List<UI_Base> _uiPrefabs = new List<UI_Base>();
    Dictionary<Type, List<UI_Base>> _uiDics = new Dictionary<Type,List<UI_Base>>();
    
    public void Awake()
    {
        Init();
    }


    public void Init()
    {
        CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.matchWidthOrHeight = 1f;
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
    }



    public T ShowUI<T>(string name = null) where T : UI_Base
    {
        Type type = typeof(T);
        T ui = null;
        T[] uiInCanvas = canvas.GetComponentsInChildren<T>();
        
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
}
