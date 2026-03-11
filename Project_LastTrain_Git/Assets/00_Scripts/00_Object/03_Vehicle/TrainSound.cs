using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSound : MonoBehaviour
{
    [SerializeField] AudioClip _trainStartSound;
    [SerializeField] AudioClip _trainRunningSound;
   

    public void PlayTrainStartSound()
    {
        SoundManager.Instance.PlaySFX(_trainStartSound);
    }

    public void PlayTrainRunningSound()
    {
        SoundManager.Instance.PlaySFXLoop(_trainRunningSound);
    }
    public float GetTrainStartSoundClipLength()
    {
        return _trainStartSound.length;
    }

}
