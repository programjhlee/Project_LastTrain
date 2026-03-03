using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [SerializeField] Animator _playerAnim;
    CollideChecker _collideChecker;
    readonly int JUMP = Animator.StringToHash("Jump");
    readonly int MOVE = Animator.StringToHash("IsMove");
    readonly int ISLAND = Animator.StringToHash("IsLand");
    readonly int DODGE = Animator.StringToHash("Dodge");
    readonly int ATTACK = Animator.StringToHash("Attack");

    public void Init()
    {
        _collideChecker = GetComponent<CollideChecker>();
        Debug.Log(_playerAnim);
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
    }
    public void PlayAnimAttack()
    {
        _playerAnim.SetTrigger(ATTACK);
    }
    public void StopAnimMove()
    {
        _playerAnim.SetBool(MOVE, false);
    }

}
