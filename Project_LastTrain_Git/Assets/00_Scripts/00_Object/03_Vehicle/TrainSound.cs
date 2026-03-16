using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSound : MonoBehaviour
{
    AudioSource _trainAudioSource;
    [SerializeField] AudioClip _trainStartSound;
    [SerializeField] AudioClip _trainRunningSound;
    [SerializeField] AudioClip _trainStopSound;
   

    public void Awake()
    {
        _trainAudioSource = GetComponent<AudioSource>();
    }

    public void OnEnable()
    {
        GameManager.Instance.OnStageStart += PlayTrainStartSound;
        GameManager.Instance.OnStageStart += PlayTrainRunningSound;
        GameManager.Instance.OnStageClear += PlayTrainStopSound;
    }

    public void OnDisable()
    {
        GameManager.Instance.OnStageStart -= PlayTrainStartSound;
        GameManager.Instance.OnStageStart -= PlayTrainRunningSound;
        GameManager.Instance.OnStageClear -= PlayTrainStopSound;
    }

    public void PlayTrainStartSound()
    {
        _trainAudioSource.PlayOneShot(_trainStartSound);
    }

    public void PlayTrainRunningSound()
    {
        _trainAudioSource.clip = _trainRunningSound;
        _trainAudioSource.loop = true;
        _trainAudioSource.Play();
    }
    public void PlayTrainStopSound()
    {
        _trainAudioSource.Stop();
        _trainAudioSource.PlayOneShot(_trainStopSound);

    }

    public float GetTrainStartSoundClipLength()
    {
        return _trainStartSound.length;
    }

}
