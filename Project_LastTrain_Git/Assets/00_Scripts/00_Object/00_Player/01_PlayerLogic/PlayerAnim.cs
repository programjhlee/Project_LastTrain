using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [SerializeField] Animator _playerAnim;
    [SerializeField]GameObject _dodgeEffect;

    CollideChecker _collideChecker;
    readonly int JUMP = Animator.StringToHash("Jump");
    readonly int MOVE = Animator.StringToHash("IsMove");
    readonly int ISLAND = Animator.StringToHash("IsLand");
    readonly int DODGE = Animator.StringToHash("Dodge");
    readonly int ATTACK = Animator.StringToHash("Attack");
    readonly int HIT = Animator.StringToHash("Hit");

    public void Init()
    {
        _collideChecker = GetComponent<CollideChecker>();
        _dodgeEffect.SetActive(false);
    }

    public void OnUpdate()
    {
        _playerAnim.SetBool(ISLAND, _collideChecker.IsLanding);
    }
    public void PlayAnimJump()
    {
        _playerAnim.SetTrigger(JUMP);
    }
    public void PlayAnimMove()
    {
        _playerAnim.SetBool(MOVE,true);
    }

    public void PlayAnimDodge()
    {
        _playerAnim.SetTrigger(DODGE);
        _dodgeEffect.SetActive(true);
    }
    public void PlayAnimAttack()
    {
        _playerAnim.SetTrigger(ATTACK);
    }
    public void PlayAnimHit()
    {
        _playerAnim.SetTrigger(HIT);
    }
    public void StopAnimMove()
    {
        _playerAnim.SetBool(MOVE, false);
    }

}
