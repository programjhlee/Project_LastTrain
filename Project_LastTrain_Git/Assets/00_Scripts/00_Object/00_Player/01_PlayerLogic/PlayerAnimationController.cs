using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Animator _playerAnim;
    [SerializeField] List<Renderer> _playerRend;
    [SerializeField] GameObject _dodgeEffect;
    [SerializeField] GameObject _fixEffect;
    PlayerAction _playerAction;
    
    readonly int JUMP = Animator.StringToHash("Jump");
    readonly int MOVE = Animator.StringToHash("IsMove");
    readonly int ISLAND = Animator.StringToHash("IsLand");
    readonly int DODGE = Animator.StringToHash("Dodge");
    readonly int ATTACK = Animator.StringToHash("Attack");
    readonly int HIT = Animator.StringToHash("Hit");

    float attackSpeed = 0.3f;
    float _attackAnimClipLength;

    public void Init()
    {
        _playerAction = GetComponentInParent<PlayerAction>();
        foreach(var clip in _playerAnim.runtimeAnimatorController.animationClips)
        {
            if (clip.name == "Melee_1H_Attack_Jump_Chop")
            {
               _attackAnimClipLength = clip.length;
            }
        }
        _dodgeEffect.SetActive(false);

        _playerAction.OnJump += PlayAnimJump;
        _playerAction.OnLand += OnPlayerLand;
        _playerAction.OnSwing += PlayAnimAttack;
        _playerAction.OnAttack += PlayAnimAttack;
        _playerAction.OnFix += PlayAnimAttack;
        _playerAction.OnHit += PlayAnimHit;
        _playerAction.OnDodge += PlayAnimDodge;

    }
    public void OnAnimationUpdate()
    {
        PlayAnimMove(_playerAction.IsMoving);
    }
    public void PlayAnimJump()
    {
        _playerAnim.SetTrigger(JUMP);
        _playerAnim.SetBool(ISLAND, false);
    }
    public void OnPlayerLand()
    {
        _playerAnim.SetBool(ISLAND, true);
    }

    public void PlayAnimMove(bool isMoving)
    {
        _playerAnim.SetBool(MOVE,isMoving);
    }

    public void PlayAnimDodge()
    {
        _playerAnim.SetTrigger(DODGE);
        _dodgeEffect.SetActive(true);
    }
    public void PlayAnimAttack()
    {
        _playerAnim.SetTrigger(ATTACK);
        _playerAnim.SetFloat("AttackSpeed", _attackAnimClipLength / (attackSpeed + 0.1f));
    }
    public void PlayAnimHit()
    {
        _playerAnim.SetTrigger(HIT);
        StartCoroutine(ColorAnim());
    }

    public IEnumerator ColorAnim()
    {
        for(int i = 0; i < _playerRend.Count; i++)
        {
            _playerRend[i].material.color = Color.red;
        }
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < _playerRend.Count; i++)
        {
            _playerRend[i].material.color = Color.white;
        }
    }
}
