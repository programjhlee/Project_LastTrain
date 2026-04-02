using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UI_BigEventCaution : UI_Base
{
    [SerializeField] TextMeshProUGUI UI_distanceText;
    [SerializeField] RectTransform _imageRect;

    public void SetDistanceText(float distance)
    {
        UI_distanceText.text = $"{distance:F2}M LEFT!!";
    }
    
    public override void Show()
    {
        base.Show();
        _imageRect.DOShakeAnchorPos(90f, 3f,10);
    }
}
