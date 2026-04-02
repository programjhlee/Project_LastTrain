using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieSoundController : MonoBehaviour
{
    
    [SerializeField] List<AudioClip> _detectSounds;
    [SerializeField] List<AudioClip> _chaseSounds;
    [SerializeField] List<AudioClip> _damageSounds;
    [SerializeField] AudioClip _deadSound;
    [SerializeField] AudioClip _attackSound;
    [SerializeField] AudioClip _spawnSound;

    List<AudioClip> _currentAudioList;
    float _curTime = 0;
    float _playSoundTimer = 5f;
    Zombie zombie;

    public void Awake()
    {
        zombie = GetComponentInParent<Zombie>();
    }
    public void Init()
    {
        zombie.OnChase += PlayChaseSound;
        zombie.OnDamaged += PlayDamageSound;
        zombie.OnDied += PlayDeadSound;
        zombie.OnAttack += PlayAttackSound;
        zombie.OnSpawn += PlaySpawnSound;
        _currentAudioList = _detectSounds;
    }

    public void OnSoundUpdate()
    {
        _curTime += Time.deltaTime;
        if(_curTime >= _playSoundTimer)
        {
            int rnd = Random.Range(0, _currentAudioList.Count);
            _curTime = 0;
            SoundManager.Instance.PlaySFX(_currentAudioList[rnd]);
        }
    }

    public void PlayAttackSound()
    {
        SoundManager.Instance.PlaySFX(_attackSound);
    }

    public void PlaySpawnSound()
    {
        SoundManager.Instance.PlaySFX(_spawnSound);
    }
    public void PlayChaseSound()
    {
        int rnd = Random.Range(0, _chaseSounds.Count);
        SoundManager.Instance.PlaySFX(_chaseSounds[rnd]);
        _currentAudioList = _chaseSounds;
    }
    public void PlayDamageSound()
    {
        int rnd = Random.Range(0, _damageSounds.Count);
        SoundManager.Instance.PlaySFX(_damageSounds[rnd]);
    }

    public void PlayDeadSound(Zombie _)
    {
        SoundManager.Instance.PlaySFX(_deadSound);
    }

    public void ResetSoundController()
    {
        zombie.OnChase -= PlayChaseSound;
        zombie.OnDamaged -= PlayDamageSound;
        zombie.OnDied -= PlayDeadSound;
        _currentAudioList = null;
    }
}
