using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Option : UI_Popup
{

    public void SetBgmVolume(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
    }

    public void SetSfxVolume(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }

}
