using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class UI_Title : UI_Base
{
    [SerializeField] TextMeshProUGUI _title;
    public override void Show()
    {
        _title.alpha = 0f;
        base.Show();
        _title.DOFade(1f,1f);
    }
    public override void Hide()
    {
        _title.DOFade(0f, 1f).OnComplete(()=>base.Hide());
    }
}
