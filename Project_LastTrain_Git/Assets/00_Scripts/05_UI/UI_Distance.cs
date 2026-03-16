using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Distance : UI_Base
{
    [SerializeField] TextMeshProUGUI ui_distanceText;

    public void SetDistanceText(float distance)
    { 
        ui_distanceText.text = $"{distance:F2}M Left!!";
        DoMoveShake();
    }

    public void DoMoveShake()
    {
        transform.DOShakePosition(0.2f,1,50).Loops();
    }
}
