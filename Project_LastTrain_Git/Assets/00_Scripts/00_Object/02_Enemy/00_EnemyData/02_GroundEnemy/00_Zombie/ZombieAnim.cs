using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieAnim : MonoBehaviour
{
    [SerializeField] Renderer _zombieRends;
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
        Debug.Log(_zombieRends);
        _zombie.OnDamaged += PlayZombieDamaged;
        _zombie.OnDied += PlayZombieDead;
        _zombie.OnAttack += PlayZombieAttack;
        foreach(var clip in _animator.runtimeAnimatorController.animationClips)
        {
            if(clip.name == "zombie_attackshorts")
            {
                attackClipLength = clip.length;
            }
        }
    }

    public void PlayZombieAttack()
    {
        _animator.SetTrigger(ATTACK);
        _animator.SetFloat("AttackSpeed", attackClipLength * _zombie.enemyData.attackSpeed);
    }

    public void PlayZombieDamaged()
    {
        _animator.SetTrigger(DAMAGED);
        StartCoroutine(DamagedColorAnim());
        
    }

    public IEnumerator DamagedColorAnim()
    {
        Material[] mats= _zombieRends.materials;
        
        for(int i = 0; i < mats.Length; i++) 
        {
            Debug.Log(mats[i].name);
            mats[i].color = Color.red;
        }
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < mats.Length; i++)
        {
            Debug.Log(mats[i].name);
            mats[i].color = Color.white;
        }
    }


    public void PlayZombieDead(Enemy _)
    {
        _animator.SetTrigger(DEAD);
    }
}
