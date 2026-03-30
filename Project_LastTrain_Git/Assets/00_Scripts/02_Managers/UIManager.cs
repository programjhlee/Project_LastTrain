using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class UIManager : SingletonManager<UIManager>
{
    [SerializeField] Image _fadeOutImage;
    [SerializeField] Canvas canvas;
    [SerializeField] Canvas canvasHUD;

    [SerializeField] List<UI_Base> _uiPrefabs;
    [SerializeField] List<UI_HUD> _uiHUDPrefabs;
    [SerializeField] List<UI_Popup> _uiPopupPrefabs;
    [SerializeField] UI_Announce _uiAnnounce;


    Dictionary<Type, List<UI_Base>> _uiDics = new Dictionary<Type,List<UI_Base>>();
    Dictionary<Type, List<UI_HUD>> _uiHUDDics = new Dictionary<Type,List<UI_HUD>>();
    Dictionary<Type, UI_Popup> _uiPopupDics = new Dictionary<Type,UI_Popup>();

    Stack<UI_Popup> _uiPopupStack;
    public void Awake()
    {
        Init();
    }


    public void Init()
    {
        _uiPopupStack = new Stack<UI_Popup>();
        for (int i = 0; i < _uiPrefabs.Count; i++)
        {
            Type type = _uiPrefabs[i].GetType();
            if (!_uiDics.ContainsKey(type))
            {
                _uiDics[type] = new List<UI_Base>();
            }
            _uiDics[type].Add(_uiPrefabs[i]);
        }
        for (int i = 0; i < _uiHUDPrefabs.Count; i++)
        {
            Type type = _uiHUDPrefabs[i].GetType();
            if (!_uiDics.ContainsKey(type))
            {
                _uiHUDDics[type] = new List<UI_HUD>();
            }
            _uiHUDDics[type].Add(_uiHUDPrefabs[i]);
        }
        for(int i = 0; i < _uiPopupPrefabs.Count; i++)
        {
            Type type = _uiPopupPrefabs[i].GetType();
            _uiPopupDics[type] = _uiPopupPrefabs[i];
        }
    }

    public T GetUI<T>(string name = null) where T : UI_Base
    {
        Type type = typeof(T);
        T ui = null;
        T[] uiInCanvas = canvas.GetComponentsInChildren<T>(true);

        for (int i = 0; i < uiInCanvas.Length; i++)
        {
            if (name == null)
            {
                ui = uiInCanvas[0];
            }
            else if (uiInCanvas[i].name == name)
            {
                ui = uiInCanvas[i];
            }
        }
        return ui;
    }

    public T ShowUI<T>(string name = null) where T : UI_Base
    {
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
            if(_uiDics.TryGetValue(typeof(T),out List<UI_Base> uis))
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

    public T ShowPopupUIAt<T>(Vector2 pos) where T : UI_Popup
    {
        T ui = null;
        if(_uiPopupDics.TryGetValue(typeof(T), out UI_Popup popupUI))
        {
            if(popupUI != null)
            {
                ui = Instantiate(popupUI.gameObject, pos, popupUI.transform.rotation).GetComponent<T>();
                ui.transform.SetParent(canvas.transform);
                ui.Show();
                ui.GetComponent<RectTransform>().anchoredPosition = pos;
            }
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
        }
    }

    public T ShowUIHUD<T>(Transform target, float upDirScale = 1.5f, string uiname = null) where T : UI_HUD
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
                    ui.Bind(canvasHUD, target, upDirScale);
                    ui.Show();
                    break;
                }
                if (uis[i].name == uiname)
                {
                    ui = Instantiate(uis[i]).GetComponent<T>();
                    ui.name = name;
                    ui.Bind(canvasHUD,target, upDirScale);
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
 
        }
    }
    public void FadeIn()
    {
        _fadeOutImage.gameObject.SetActive(true);
        _fadeOutImage.transform.SetAsLastSibling();
        _fadeOutImage.DOFade(0f, 0.5f).OnComplete(()=>_fadeOutImage.gameObject.SetActive(false));
    }
    public void FadeOut()
    {
        _fadeOutImage.gameObject.SetActive(true);
        _fadeOutImage.transform.SetAsLastSibling();
        _fadeOutImage.DOFade(1f, 0.5f);
    }

    public UI_Announce ShowAnnounce(IAnnounceStrategy strategy , string announceText, Vector3 pos)
    {
        _uiAnnounce.SetUIStrategy(strategy);
        _uiAnnounce.SetAnnounceText(announceText);
        UI_Announce ui = Instantiate(_uiAnnounce, pos, _uiAnnounce.transform.rotation).GetComponent<UI_Announce>();
        ui.transform.SetParent(canvas.transform);
        ui.Show();
        ui.GetComponent<RectTransform>().anchoredPosition = pos;
        return ui;
    }
}
