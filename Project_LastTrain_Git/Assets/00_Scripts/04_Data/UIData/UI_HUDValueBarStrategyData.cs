using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UI_HUDStrategy",menuName = "Create UI_HUD_StrategyData")]
public class UI_HUDStrategyData : ScriptableObject
{
    [SerializeField] Color _fillColor;
    [SerializeField] Vector2 _uiHudSize;

    public Color FillColor
    {
        get
        {
            return _fillColor; 
        }
        private set
        {
            _fillColor = value;
        }
    }
    public Vector2 UIHUDSize
    {
        get
        {
            return _uiHudSize;
        }
        private set
        {
            _uiHudSize = value;
        }
    } 
}
