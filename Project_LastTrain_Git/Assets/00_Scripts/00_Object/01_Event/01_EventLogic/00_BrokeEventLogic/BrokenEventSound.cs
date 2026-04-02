using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenEventSound : MonoBehaviour
{
    AudioSource _audioSource;
    [SerializeField] AudioClip _brokenEventEnterSound;
    [SerializeField] AudioClip _fixedEventSound;

    public void OnAwake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public float GetfixedAudioClipLength()
    {
        return _fixedEventSound.length;
    }
    public void PlayEnterSound()
    {
        _audioSource.clip = _brokenEventEnterSound;
        _audioSource.Play();
    }

    public void PlayExitSound()
    {
        _audioSource.Stop();
        SoundManager.Instance.PlaySFX(_fixedEventSound);
    }

}
