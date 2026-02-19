using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : SingletonManager<UIManager>
{

    [SerializeField] Canvas canvas;
    [SerializeField] List<UI_Base> _uiPrefabs = new List<UI_Base>();
    Dictionary<Type, UI_Base> _uiDics = new Dictionary<Type,UI_Base>();
    
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
                _uiDics[type] = _uiPrefabs[i];
            }
        }
    }



    public T ShowUI<T>() where T : UI_Base
    {
        Type type = typeof(T);
        T ui = null;
        if (!_uiDics.TryGetValue(type, out UI_Base value))
        {
            return null;
        }
        else
        {
            if (canvas.transform.GetComponentsInChildren<T>(true).Length <= 0)
            {
                ui = Instantiate(_uiDics[type]).GetComponent<T>();
                ui.transform.SetParent(canvas.transform);
                ui.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                ui.Show();
            }
            else
            {
                foreach (var uiInCanvas in canvas.transform.GetComponentsInChildren<T>(true))
                {
                    if(uiInCanvas.GetType() == type)
                    {
                        ui = uiInCanvas;
                        ui.Show();
                    }
                }
            }
        }
        return ui;
    }

    public T ShowUIAt<T>(Vector2 pos) where T : UI_Base
    {
        T ui = ShowUI<T>();
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
