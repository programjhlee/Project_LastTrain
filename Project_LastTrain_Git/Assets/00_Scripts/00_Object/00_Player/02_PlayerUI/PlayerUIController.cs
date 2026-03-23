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
    Collider _col;
    Dictionary<string,UI_HUDControlGuideStrategyData> _controlGuideDics;
    UI_HUDControlGuide _uiControlGuide;


    public void OnDisable()
    {
        HideControlGuide();
    }
    public void Init()
    {
        _col = GetComponent<Collider>();
        _uiControlGuide = UIManager.Instance.ShowUIHUD<UI_HUDControlGuide>(transform);
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
    public void ShowControlGuide(UI_HUDControlGuideStrategyData strategyData)
    {
        Debug.Log("strateData 업데이트");
        _uiControlGuide.BindData(strategyData);
        _uiControlGuide.Show();
    }
    public void CheckInteraction()
    {
        RaycastHit hit;
        if (Physics.BoxCast(_col.bounds.center, new Vector3(0.5f, 0.5f, 0.5f), transform.forward, out hit, Quaternion.identity, 2f))
        {
            if (hit.collider.TryGetComponent<IAttackable>(out IAttackable enemy) || hit.collider.TryGetComponent<IFixable>(out IFixable fixable) ||
                hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                ShowControlGuide(ControlGuideType.Interaction);
            }
        }
        else
        {
            HideControlGuide();
        }
    }


    public void HideControlGuide()
    {
        _uiControlGuide.Hide();
    }

    public void UIUpdate()
    {
        _uiControlGuide.UpdatePos();
        CheckInteraction();
    }
}
