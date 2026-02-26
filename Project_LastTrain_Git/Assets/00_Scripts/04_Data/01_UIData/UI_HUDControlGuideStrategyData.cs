using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UI_HUDControlGuide", menuName = "Create UI_HUD_StrategyData/UI_HUDControlGuide")]
public class UI_HUDControlGuideStrategyData : ScriptableObject
{
    [SerializeField] Sprite _controlGuideKeyImage;
    [SerializeField] string _controlGuideText;

    public Sprite ControlGuideKeyImage
    {
        get
        {
            return _controlGuideKeyImage;
        }
        private set
        {
            _controlGuideKeyImage = value;
        }
    }
    public string ControlGuideText
    {
        get
        {
            return _controlGuideText;
        }
        private set
        {
            _controlGuideText = value;
        }
    }
}
