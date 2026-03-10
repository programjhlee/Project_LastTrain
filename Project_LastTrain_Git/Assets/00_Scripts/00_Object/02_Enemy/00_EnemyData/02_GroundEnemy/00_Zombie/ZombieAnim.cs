using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnim : MonoBehaviour
{
    Zombie _zombie;
    Animator _animator;

    readonly int DEAD = Animator.StringToHash("Dead");
    readonly int ATTACK = Animator.StringToHash("Attack");
    readonly int DAMAGED= Animator.StringToHash("Damaged");
    readonly int MOVE = Animator.StringToHash("Move");
    float attackClipLength;

    public void Init()
    {
        _zombie = GetComponent<Zombie>();
        _animator = GetComponentInChildren<Animator>();
        _animator.SetTrigger(MOVE);
        _zombie.OnDamaged += PlayZombieDamaged;
        _zombie.OnDied += PlayZombieDead;
        _zombie.OnAttack += PlayZombieAttack;
        foreach(var clip in _animator.runtimeAnimatorController.animationClips)
        {
            Debug.Log(clip.name);
            if(clip.name == "zombie_attackshorts")
            {
                attackClipLength = clip.length;
            }
        }
        Debug.Log(attackClipLength);
    }

    public void PlayZombieAttack()
    {
        _animator.SetTrigger(ATTACK);
        _animator.SetFloat("AttackSpeed", attackClipLength * _zombie.enemyData.attackSpeed);
    }

    public void PlayZombieDamaged()
    {
        _animator.SetTrigger(DAMAGED);
    }

    public void PlayZombieDead(Enemy _)
    {
        _animator.SetTrigger(DEAD);
    }
}
