using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHUDController : MonoBehaviour
{
    List<UI_HUD> _uiHUDList = new List<UI_HUD>();

    public void AddUIHUD(UI_HUD uiHUD)
    {
        _uiHUDList.Add(uiHUD);
    }

    public void UpdateUIHUDPos()
    {
        for(int i = 0; i < _uiHUDList.Count; i++)
        {
            _uiHUDList[i].SetUpDirScale((i + 1) * 1.2f);
            _uiHUDList[i].UpdatePos();
        }
    }

    public void UIHUDListClear()
    {
        for(int i = 0; i < _uiHUDList.Count; i++)
        {
            Destroy(_uiHUDList[i].gameObject);
        }
        _uiHUDList.Clear();
    }


}
