using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSound : MonoBehaviour
{
    AudioSource _trainAudioSource;
    [SerializeField] AudioClip _trainStartSound;
    [SerializeField] AudioClip _trainRunningSound;
    [SerializeField] AudioClip _trainStopSound;
    Train _train;
   

    public void Awake()
    {
        _train = GetComponent<Train>();
        Debug.Log(_train);
        _trainAudioSource = GetComponent<AudioSource>();
    }

    public void OnEnable()
    {
        GameManager.Instance.OnStageStart += PlayTrainStartSound;
        GameManager.Instance.OnStageStart += PlayTrainRunningSound;
        GameManager.Instance.OnStageClear += PlayTrainStopSound;
        _train.OnReset += StopTrainRunningSound;
    }
    public void OnDisable()
    {
        GameManager.Instance.OnStageStart -= PlayTrainStartSound;
        GameManager.Instance.OnStageStart -= PlayTrainRunningSound;
        GameManager.Instance.OnStageClear -= PlayTrainStopSound;
        _train.OnReset -= StopTrainRunningSound;
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
    public void StopTrainRunningSound()
    {
        _trainAudioSource.Stop();
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
