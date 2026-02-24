using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAction : MonoBehaviour,IGravityAffected
{

    PlayerData playerData;

    private LayerMask playerLayer = 6;
    private int enemyLayer = 7;
    private LayerMask groundLayer = 3;

    Collider col;

    LandChecker landChecker;

    bool isHit;
    bool cantHit;
    bool canRolling;
    bool canInteraction;
    public Vector3 MoveDir { get; set; }
    public float JumpVel { get; set; }

    public float RollingSpeed { get; set; }

    public event Action OnMove;
    public event Action OnJump;
    public event Action OnAttack;
    public event Action OnFix;
    public event Action OnRoll;
    
    public void Init()
    {
        GravityManager.Instance.AddGravityObj(gameObject.GetComponent<IGravityAffected>());
        landChecker = GetComponent<LandChecker>();
        playerData = GetComponent<PlayerData>();
        col = GetComponent<Collider>();
        
        playerData.MoveSpeed =  7f;
        playerData.JumpForce = 15f;
        playerData.AttackPower = 5f;
        playerData.FixPower = 2f;
        RollingSpeed = 20f;
        
        canInteraction = true;
        canRolling = true;
        
    }

    public void Move(Vector3 moveDir)
    {
        if (isHit)
        {
            MoveDir = Vector3.zero;
            return;
        }
        MoveDir = moveDir;
    }

    public void ProcessMovement()
    {
        if (GameManager.Instance.IsPaused())
        {
            MoveDir = Vector3.zero;
            return;
        }

        if (MoveDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(MoveDir);
            transform.rotation = targetRot;
            transform.position += MoveDir * playerData.MoveSpeed * Time.fixedDeltaTime;
            OnMove?.Invoke();
        }

        if (Physics.Raycast(transform.position, transform.forward, col.bounds.extents.x + 0.1f, (1 << enemyLayer)))
        {
            MoveDir = Vector3.zero;
            return;
        }
        transform.position += Vector3.up * JumpVel * Time.fixedDeltaTime;
        if (transform.position.y < -20f)
        {
            transform.position = new Vector3(0, -1f, 0);
            StartCoroutine(DamageProcess(Vector3.zero));
        }
    }
    public void Jump()
    {
        if (landChecker.IsLanding)
        {
            JumpVel = playerData.JumpForce;
            OnJump?.Invoke();
        }
    }

    public void Interaction()
    {
        if (!canInteraction || isHit)
        {
            return;
        }
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2f))
        {
            if (hit.collider.TryGetComponent<IAttackable>(out IAttackable enemy))
            {
                Attack(enemy, hit.collider.transform.position - transform.position);
            }
            if (hit.collider.TryGetComponent<IFixable>(out IFixable fixable))
            {
                Fix(fixable);
            }
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                Interaction(interactable);
            }
        }
        StartCoroutine(InteractionCoolTimeProcess(0.3f));
    }
    public void Attack(IAttackable _attackable, Vector3 attackDir)
    {
        _attackable.TakeDamage(playerData.AttackPower, attackDir);
    }

    public void Fix(IFixable _fixable)
    {
        _fixable.TakeFix(playerData.FixPower);
    }

    public void Rolling()
    {
        if (MoveDir == Vector3.zero || canRolling == false)
        {
            return;
        }
        OnRoll?.Invoke();
        StartCoroutine(RollingProcess());
    }
    public void Interaction(IInteractable interactable)
    {
        interactable.Interact();
    }
        
    public void TakeDamage(Vector3 dir)
    {
        if (cantHit)
        {
            return;
        }
        StartCoroutine(DamageProcess(dir));
    }

    IEnumerator InteractionCoolTimeProcess(float interactionCoolTime)
    {
        canInteraction = false;
        yield return new WaitForSeconds(interactionCoolTime);
        canInteraction = true;
    }

    IEnumerator RollingProcess()
    {
        cantHit = true;
        canRolling = false;
        Physics.IgnoreLayerCollision(playerLayer, enemyLayer,true);
        Vector3 target = transform.position + MoveDir * 3f;
        while (Vector3.Distance(transform.position,target)>0.0001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, RollingSpeed * Time.deltaTime);
            yield return null;
        }
        cantHit = false;
        Physics.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        yield return new WaitForSeconds(1f);
        canRolling = true;
    }


    IEnumerator DamageProcess(Vector3 dir)
    {
        isHit = true;
        float attackForce = 10f;
        float curTime = 0;
        while (curTime <= 0.1f)
        {
            curTime += Time.deltaTime;
            transform.position += new Vector3(dir.x, 0.4f, 0) * attackForce * Time.deltaTime;
            yield return null;
        }
        curTime = 0;
        cantHit = true;
        curTime = 0;
        Renderer rend = GetComponent<Renderer>();
        while(curTime <= 1f)
        {
            if(curTime >= 0.25f)
            {
                isHit = false;
            }
            curTime += Time.deltaTime;
            rend.enabled = false;
            yield return null;
            rend.enabled = true;
            yield return null;
        }
        cantHit = false;
    }

    public void AffectGravity(float gravity)
    {
        if (!landChecker.IsLanding)
        {
            JumpVel -= gravity * Time.fixedDeltaTime;
        }
        else
        {
            if (JumpVel < 0)
            {
                JumpVel = 0;
                transform.position = new Vector3(transform.position.x, landChecker.GetLandYPos(), 0);
            }
        }
    }
}
