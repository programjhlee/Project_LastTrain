using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Distance : UI_Base
{
    [SerializeField] Text ui_distanceText;

    public void SetDistanceText(float distance)
    { 
        ui_distanceText.text = $"{distance:F2}m Left!";
    }
}
