using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAction : MonoBehaviour,IGravityAffected
{
    [SerializeField] Tool _tool;
    PlayerData _playerData;
    Collider _col;
    LandChecker _landChecker;

    WaitForSeconds _rollingCoolTime = new WaitForSeconds(1f);

    public event Action OnMove;
    public event Action OnJump;
    public event Action OnAttack;
    public event Action OnFix;
    public event Action OnInteraction;
    public event Action OnRoll;

    public float YVel { get { return _yVel; } set { _yVel = value; }}

    public Transform TargetTransform { get { return transform; } }

    public LandChecker LandChecker { get { return _landChecker; } }

    LayerMask _playerLayer = 6;
    int _enemyLayer = 7;

    float _yVel;

    Vector3 _moveDir;




    bool _isHit;
    bool _canHit;
    bool _canRolling;
    bool _canInteraction;

    
    public void Init(PlayerData playerData)
    {
        GravityManager.Instance.AddGravityObj(gameObject.GetComponent<IGravityAffected>());

        _landChecker = GetComponent<LandChecker>();
        _col = GetComponent<Collider>();
        _tool.Init();
        
        _playerData = playerData;
        _yVel = 0;

        _isHit = false;
        _canHit = true;
        _canInteraction = true;
        _canRolling = true;
    }
    public void SetMoveDirection(Vector3 moveDir)
    {
        if (_isHit)
        {
            _moveDir = Vector3.zero;
            return;
        }
        _moveDir = moveDir;
    }

    public void ProcessMovement()
    {
        if (GameManager.Instance.IsPaused())
        {
            SetMoveDirection(Vector3.zero);
            return;
        }

        RotateToward(_moveDir);

        if (IsFrontBlockedBy(_enemyLayer))
        {
            SetMoveDirection(Vector3.zero);
        }

        else
        {
            Move(_moveDir);
        }
        Apply_yVel();
    }

    public void Move(Vector3 moveDir)
    {
        if (_moveDir != Vector3.zero)
        {
            transform.position += _moveDir * _playerData.MoveSpeed * Time.fixedDeltaTime;
            OnMove?.Invoke();
        }
    }

    public void RotateToward(Vector3 dir)
    {
        if (_moveDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = targetRot;
        }
    }

    public bool IsFrontBlockedBy(int layerMask)
    {
        return Physics.Raycast(transform.position, transform.forward, _col.bounds.extents.x + 0.1f, (1 << layerMask));
    }

    public void Jump()
    {
        if (_landChecker.IsLanding)
        {
            _yVel = _playerData.JumpForce;
            OnJump?.Invoke();
        }
    }

    public void Interaction()
    {
        if (!_canInteraction || _isHit)
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
                OnAttack?.Invoke();
            }
            if (hit.collider.TryGetComponent<IFixable>(out IFixable fixable))
            {
                Fix(fixable);
                OnFix?.Invoke();
            }
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                Interaction(interactable);
                OnInteraction?.Invoke();
            }
        }
        StartCoroutine(InteractionCoolTimeProcess(0.3f));
    }
    void Attack(IAttackable _attackable, Vector3 attackDir)
    {
        _attackable.TakeDamage(_playerData.AttackPower, attackDir);
    }
    
    void Fix(IFixable _fixable)
    {
        _fixable.TakeFix(_playerData.FixPower);
    }

    public void Rolling()
    {
        if (_moveDir == Vector3.zero || _canRolling == false)
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
        if (!_canHit)
        {
            return;
        }
        StartCoroutine(GetDamageSequence(dir));
    }

    IEnumerator InteractionCoolTimeProcess(float interactionCoolTime)
    {
        _canInteraction = false;
        yield return new WaitForSeconds(interactionCoolTime);
        _canInteraction = true;
    }

    IEnumerator RollingProcess()
    {
        _canHit = false;
        _canRolling = false;
        Physics.IgnoreLayerCollision(_playerLayer, _enemyLayer,true);
        Vector3 target = transform.position + _moveDir * 3f;
        while (Vector3.Distance(transform.position,target) > 0.0001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, _playerData.RollingSpeed * Time.deltaTime);
            yield return null;
        }
        _canHit = true;
        Physics.IgnoreLayerCollision(_playerLayer, _enemyLayer, false);
        yield return _rollingCoolTime;
        _canRolling = true;
    }


    IEnumerator GetDamageSequence(Vector3 dir)
    {
        _isHit = true;
        _canHit = false;

        float attackForce = 10f;
        float stunTime = 0.25f;
        float iframeTime = 1f;

        yield return StartCoroutine(ForceOut(dir, attackForce));
        yield return new WaitForSeconds(stunTime);

        _isHit = false;

        yield return StartCoroutine(IFrameDuration(iframeTime));

        _canHit = true;
    }
    IEnumerator ForceOut(Vector3 dir, float attackForce)
    {
        float curTime = 0;

        while (curTime <= 0.1f)
        {
            curTime += Time.deltaTime;
            transform.position += new Vector3(dir.x, 0.4f, 0) * attackForce * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator IFrameDuration(float duration)
    {
        float curTime = 0;

        Renderer rend = GetComponent<Renderer>();
        while (curTime <= duration)
        {
            curTime += Time.deltaTime;

            rend.enabled = false;

            yield return null;

            rend.enabled = true;

            yield return null;
        }
    }

    void Apply_yVel()
    {
        transform.position += Vector3.up * _yVel * Time.fixedDeltaTime;
        if (transform.position.y < -20f)
        {
            transform.position = new Vector3(0, -1f, 0);
            StartCoroutine(GetDamageSequence(Vector3.zero));
        }
    }
}
