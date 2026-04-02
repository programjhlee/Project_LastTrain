using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UI_HUDStrategy",menuName = "Create UI_HUD_StrategyData")]
public class UI_HUDValueBarStrategyData : ScriptableObject
{
    [SerializeField] Sprite _fillSprite;
    [SerializeField] Vector2 _uiHudSize;

    public Sprite FillSprite
    {
        get
        {
            return _fillSprite ; 
        }
        private set
        {
            _fillSprite = value;
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
