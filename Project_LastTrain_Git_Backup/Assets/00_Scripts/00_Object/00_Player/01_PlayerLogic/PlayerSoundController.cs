using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] List<AudioClip> _fixSounds;
    [SerializeField] List<AudioClip> _attackSounds;
    [SerializeField] List<AudioClip> _walkSounds;
    [SerializeField] AudioClip _dodgeSound;
    [SerializeField] AudioClip _swingSound;
    [SerializeField] AudioClip _jumpSound;
    [SerializeField] PlayerAction _playerAction;
    
    int walkIdx = 0;


    public void Init()
    {
        _playerAction = GetComponentInParent<PlayerAction>();

        _playerAction.OnJump -= PlayJumpSoundEffect;
        _playerAction.OnAttack -= PlayAttackSoundEffect;
        _playerAction.OnFix -= PlayFixSoundEffect;
        _playerAction.OnSwing -= PlaySwingSoundEffect;
        _playerAction.OnDodge -= PlayDodgeSoundEffect;

        _playerAction.OnJump += PlayJumpSoundEffect;
        _playerAction.OnAttack += PlayAttackSoundEffect;
        _playerAction.OnFix += PlayFixSoundEffect;
        _playerAction.OnSwing += PlaySwingSoundEffect;
        _playerAction.OnDodge += PlayDodgeSoundEffect;

    }
    public void PlayFixSoundEffect()
    {
        int rnd = Random.Range(0, _fixSounds.Count);
        SoundManager.Instance.PlaySFX(_fixSounds[rnd],0.4f);
    }
    public void PlayAttackSoundEffect()
    {
        int rnd = Random.Range(0, _fixSounds.Count);
        SoundManager.Instance.PlaySFX(_attackSounds[rnd], 0.4f);
    }
    public void PlayWalkSoundEffect()
    {
        walkIdx = (walkIdx + 1) % 2;
        SoundManager.Instance.PlaySFX(_walkSounds[walkIdx],0.7f);
    }
    public void PlayJumpSoundEffect()
    {
        SoundManager.Instance.PlaySFX(_jumpSound, 0.5f);
    }
    public void PlayDodgeSoundEffect()
    {
        SoundManager.Instance.PlaySFX(_dodgeSound);
    }
    public void PlaySwingSoundEffect()
    {
        SoundManager.Instance.PlaySFX(_swingSound, 0.7f);
    }
}
