using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Option : UI_Popup
{
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _sfxSlider;
    [SerializeField] Button _closeButton;
    public void SetBgmVolume(float value)
    {
        SoundManager.Instance.SetUserBGMValue(value);
    }

    public void SetSfxVolume(float value)
    {
        SoundManager.Instance.SetUserSFXValue(value);
    }
    public override void Show()
    {
        base.Show();
        _bgmSlider.value = SoundManager.Instance.UserBGMVolumeSet;
        _sfxSlider.value = SoundManager.Instance.UserSFXVolumeSet;
        _bgmSlider.interactable = true;
        _sfxSlider.interactable = true;
        _closeButton.interactable = true;
    }
    public override void Hide()
    {
        base.Hide();
 
        _bgmSlider.interactable = false;
        _sfxSlider.interactable = false;
        _closeButton.interactable = false;
    }
}
