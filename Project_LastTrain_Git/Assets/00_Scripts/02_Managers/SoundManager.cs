using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonManager<SoundManager>
{
    [SerializeField] AudioSource _BGM;
    [SerializeField] AudioSource _SFX;
    public void PlaySFX(AudioClip clip,float volume = 1)
    {
        _SFX.PlayOneShot(clip, volume);
    }
    public void PlayBGM(AudioClip clip)
    {
        _BGM.loop = true;
        _BGM.clip = clip;
        _BGM.Play();
    }
    public void StopBGM()
    {
        _BGM.Stop();
    }

    public void PauseBGM()
    {
        _BGM.Pause();
    }
    public void ResumeBGM()
    {
        _BGM.UnPause();
    }
    public void PlaySFXLoop(AudioClip clip, float volume = 1)
    {
        _SFX.clip = clip;
        _SFX.loop = true;
        _SFX.Play();
    }
    public void StopSFX()
    {
        _SFX.Stop();
    }
    public void SetAudioVolume(AudioSource audio, float volume)
    {
        audio.volume = volume;
    }
    public void SetBGMVolume(float volume)
    {
        SetAudioVolume(_BGM, volume);
    }
    public void SetSFXVolume(float volume)
    {
        SetAudioVolume(_SFX, volume);
    }
}
