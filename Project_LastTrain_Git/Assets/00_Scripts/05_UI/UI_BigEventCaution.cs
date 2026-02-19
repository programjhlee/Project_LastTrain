using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BigEventCaution : UI_Base
{
    [SerializeField] Text UI_distanceText;


    public void SetDistanceText(float distance)
    {
        UI_distanceText.text = $"{distance:F2}m Left!";
    }
    
}
