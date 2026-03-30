using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : SingletonManager<SoundManager>
{
    float _userBGMVolumeSet;
    float _userSFXVolumeSet;

    float _fadeMultiplier;

    List<AudioSource> _objAudioSources;
    public enum BGMType
    {
        Title,
        GameBGM,
        GameAllClear,
    }

    public float UserBGMVolumeSet
    {
        get
        {
            return _userBGMVolumeSet;
        }
        set
        {
            _userBGMVolumeSet = value;
        }
    }
    public float UserSFXVolumeSet
    {
        get
        {
            return _userSFXVolumeSet;
        }
        set
        {
            _userSFXVolumeSet = value;
        }
    }

    [SerializeField] List<AudioClip> _bgmSounds;
    [SerializeField] AudioSource _BGM;
    [SerializeField] AudioSource _SFX;
    Dictionary<BGMType, AudioClip> _BGMDict;


    public void Awake()
    {
        _userBGMVolumeSet = 1f;
        _userSFXVolumeSet = 1f;
        _fadeMultiplier = 1f;
        _BGMDict = new Dictionary<BGMType, AudioClip>();
        BGMType[] bgmKey = (BGMType[])Enum.GetValues(typeof(BGMType));
        for(int i = 0; i < bgmKey.Length; i++)
        {
            Debug.Log($"{bgmKey[i]} , {_bgmSounds[i]}");
            _BGMDict[bgmKey[i]] = _bgmSounds[i];
        }
    }
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
    public void PlayBGM(BGMType type)
    {
        Debug.Log("BGM ½ÃÀÛ!");
        if (_BGMDict.TryGetValue(type, out AudioClip playClip))
        {
            Debug.Log(playClip);
            PlayBGM(_BGMDict[type]);
        }
    }

    public void VolumeFadeOut(float duration = 1f , float targetVolume = 0f)
    {
        StartCoroutine(VolumeFadeOutProcess(duration, targetVolume));
    }
    public void VolumeFadeIn(float duration = 1f, float targetVolume = 1f)
    {
        StartCoroutine(VolumeFadeInProcess(duration, targetVolume));
    }
    public IEnumerator VolumeFadeOutProcess(float duration, float targetVolume)
    {
        float startVolume = _fadeMultiplier;
        if(startVolume <= targetVolume)
        {
            yield break;
        }
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            _fadeMultiplier = Mathf.Lerp(startVolume, targetVolume, t);
            elapsed += Time.deltaTime;
            _BGM.volume = _userBGMVolumeSet * _fadeMultiplier;
            yield return null;
        }
    }

    public IEnumerator VolumeFadeInProcess(float duration, float targetVolume)
    {
        float startVolume = 0.2f;
        if (startVolume >= targetVolume)
        {
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            _fadeMultiplier = Mathf.Lerp(startVolume, targetVolume, t);
            elapsed += Time.deltaTime;
            _BGM.volume = _userBGMVolumeSet * _fadeMultiplier;
            yield return null;
        }
    }

    public void AddObjAudioSource(GameObject obj)
    {
        if(_objAudioSources == null)
        {
            _objAudioSources = new List<AudioSource>();
        }
        _objAudioSources.Add(obj.GetComponent<AudioSource>());
    }

    public void StopBGM()
    {
        StopAllCoroutines();
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
    public void SetUserBGMValue(float value)
    {
        _userBGMVolumeSet = value;
        _BGM.volume = _fadeMultiplier * _userBGMVolumeSet;


    }
    public void SetUserSFXValue(float value)
    {
        _userSFXVolumeSet = value;
        _SFX.volume = _userSFXVolumeSet;
        for(int i = 0; i < _objAudioSources.Count; i++)
        {
            _objAudioSources[i].volume = _userSFXVolumeSet;
        }
    }
}
