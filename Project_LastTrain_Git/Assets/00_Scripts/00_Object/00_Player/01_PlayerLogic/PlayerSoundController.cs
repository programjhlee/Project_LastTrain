using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] List<AudioClip> _fixSounds;
    [SerializeField] List<AudioClip> _attackSounds;
    [SerializeField] List<AudioClip> _walkSounds;
    [SerializeField] AudioClip _jumpSound;
    PlayerAction _playerAction;
    int walkIdx = 0;

    public void Init()
    {
        _playerAction = GetComponentInParent<PlayerAction>();
        _playerAction.OnFix += PlayFixSoundEffect;
        _playerAction.OnAttack += PlayAttackSoundEffect;
        _playerAction.OnJump += PlayJumpSoundEffect;
    }

    public void InterActionSoundEffect()
    {
        //TO - DO 그냥 휘두를때 나는 소리
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
}
