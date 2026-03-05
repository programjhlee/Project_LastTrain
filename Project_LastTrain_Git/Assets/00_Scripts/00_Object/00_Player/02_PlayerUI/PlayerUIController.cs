using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerUIController : MonoBehaviour
{
    public enum ControlGuideType
    {
        None,
        Interaction,
    }
    
    
    [SerializeField] List<UI_HUDControlGuideStrategyData> _controlGuideList;
    Dictionary<string,UI_HUDControlGuideStrategyData> _controlGuideDics;
    UI_ControlGuide _uiControlGuide;

    public void Init()
    {
        _uiControlGuide = UIManager.Instance.ShowUIHUD<UI_ControlGuide>(transform);
        _uiControlGuide.Hide();
        _controlGuideDics = new Dictionary<string, UI_HUDControlGuideStrategyData>();
        for (int i = 0; i < _controlGuideList.Count; i++)
        {
            _controlGuideDics[_controlGuideList[i].ControlGuideName] = _controlGuideList[i];
        }
    }
    public void ShowControlGuide(ControlGuideType controlGuide)
    {
        string controlName = Enum.GetName(typeof(ControlGuideType),controlGuide);
        _uiControlGuide.BindData(_controlGuideDics[controlName]);
        _uiControlGuide.Show();
    }

    public void HideControlGuide()
    {
        _uiControlGuide.Hide();
    }

    // Update is called once per frame
    public void UIUpdate()
    {
        
        _uiControlGuide.UpdatePos();
    }
}
