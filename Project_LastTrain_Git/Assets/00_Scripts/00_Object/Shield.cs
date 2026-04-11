using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shield : MonoBehaviour
{
    [SerializeField] GameObject _shieldEffect;
    [SerializeField] AudioClip _shieldSoundEffect;
    [SerializeField] ParticleSystem _shieldParticleSystem;
    WaitForSeconds _shieldTime = new WaitForSeconds(5f);

    public void TurnOn()
    {
        StopAllCoroutines();
        _shieldParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _shieldParticleSystem.Play(true);
        SoundManager.Instance.PlaySFX(_shieldSoundEffect);
        gameObject.SetActive(true);
        _shieldEffect.SetActive(true);
        StartCoroutine(ShieldProcess(_shieldTime));
    }
    public void TurnOff()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        _shieldEffect.SetActive(false);
    }
    IEnumerator ShieldProcess(WaitForSeconds shieldTime)
    {
        yield return shieldTime;
        TurnOff();
    }
}
