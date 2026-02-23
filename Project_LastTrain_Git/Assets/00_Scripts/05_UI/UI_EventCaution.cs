using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EventCaution : UI_Base
{
    [SerializeField] Text ui_eventCntText;
    

    public void SetEventCount(int cnt)
    {
        ui_eventCntText.text = cnt.ToString();
    }
}
