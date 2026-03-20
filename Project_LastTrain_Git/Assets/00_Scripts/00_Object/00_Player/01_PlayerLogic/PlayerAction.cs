using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAction : MonoBehaviour,IGravityAffected
{
    [SerializeField] List<Renderer> _playerRend;
    [SerializeField] Tool _tool;

    PlayerData _playerData;
    Collider _col;
    CollideChecker _collideChecker;
    
    WaitForSeconds _rollingCoolTime = new WaitForSeconds(1f);

    public event Action OnJump;
    public event Action OnLand;
    public event Action OnAttack;
    public event Action OnSwing;
    public event Action OnFix;
    public event Action OnInteraction;
    public event Action OnHit;
    public event Action OnDodge;


    bool _isActive =false;
    bool _isMoving = false;

    float _yVel;

    public bool IsMoving => _isMoving;

    public bool IsActive { get { return _isActive; } set { _isActive = value; } }
    public float YVel { get { return _yVel; } set { _yVel = value; }}

    public Transform TargetTransform { get { return transform; } }

    public CollideChecker CollideChecker { get { return _collideChecker; } }

    LayerMask _playerLayer;
    LayerMask _enemyLayer;
    LayerMask _obstacleLayer;

    int _playerLayerNum;
    int _enemyLayerNum;
    int _obstacleLayerNum;

    Vector3 _moveDir;

    bool _isHit;
    bool _canHit;
    bool _canRolling;
    bool _canInteraction;
    
    public void Init(PlayerData playerData)
    {
        GravityManager.Instance.AddGravityObj(gameObject.GetComponent<IGravityAffected>());

        _collideChecker = GetComponent<CollideChecker>();
        _col = GetComponent<Collider>();

        _tool.Init();

        _playerLayer = LayerMask.GetMask("Player");
        _enemyLayer = LayerMask.GetMask("Enemy");
        _obstacleLayer = LayerMask.GetMask("Obstacle");

        _playerLayerNum = LayerMask.NameToLayer("Player");
        _enemyLayerNum = LayerMask.NameToLayer("Enemy");
        _obstacleLayerNum = LayerMask.NameToLayer("Obstacle");

        _playerData = playerData;
        PlayerDataInit(playerData);
        _yVel = 0;
        IsActive = true;

        _isHit = false;
        _canHit = true;
        _canInteraction = true;
        _canRolling = true;
    }

    public void PlayerDataInit(PlayerData playerData)
    {
        playerData.Level = 1;
        playerData.AttackPower = 5;
        playerData.FixPower = 2;
    }


    public void SetMoveDirection(Vector3 moveDir)
    {
        _moveDir = moveDir;
    }

    public void ProcessMovement()
    {
        if (!GameManager.Instance.IsGamePlaying() && !GameManager.Instance.IsTutorial())
        {
            _isMoving = false;
            SetMoveDirection(Vector3.zero);
            return;
        }
        _collideChecker.LandCheck();
        ApplyYVel();

        if (_isHit)
        {
            _isMoving = false;
            SetMoveDirection(Vector3.zero);
            return;
        }

        RotateToward(_moveDir);

        if (_collideChecker.IsFrontBlockedBy(_enemyLayer) || _collideChecker.IsFrontBlockedBy(_obstacleLayer))
        {
            _isMoving = false;
            SetMoveDirection(Vector3.zero);
            return;
        }
        if (_moveDir != Vector3.zero)
        {
            _isMoving = true;
            transform.position += _moveDir * _playerData.MoveSpeed * Time.fixedDeltaTime;
        }
        else
        {
            _isMoving = false;
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
    public void Jump()
    {
        if (_collideChecker.IsLanding)
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
        RaycastHit hit;
        OnSwing?.Invoke();

        if (Physics.BoxCast(_col.bounds.center, new Vector3(0.5f, 0.5f, 0.5f), transform.forward, out hit, Quaternion.identity, 2f))
        {
            if (hit.collider.TryGetComponent<IAttackable>(out IAttackable attackable))
            {
                attackable.TakeDamage(_playerData.AttackPower, hit.collider.transform.position - transform.position);
                OnAttack?.Invoke();
            }
            if (hit.collider.TryGetComponent<IFixable>(out IFixable fixable))
            {
                fixable.TakeFix(_playerData.FixPower);
                OnFix?.Invoke();
            }
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact();
                OnInteraction?.Invoke();
            }
        }
        StartCoroutine(InteractionCoolTimeProcess(0.3f));
    }

    public void Dodge()
    {
        if (_moveDir == Vector3.zero || _canRolling == false)
        {
            return;
        }
        OnDodge?.Invoke();
        StartCoroutine(RollingProcess());
    }
    public void TakeDamage(Vector3 dir)
    {
        if (!_canHit)
        {
            return;
        }
        OnHit?.Invoke();
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
        Physics.IgnoreLayerCollision(_playerLayerNum, _enemyLayerNum, true);
        Vector3 target = transform.position + _moveDir * 3f;
        while (Vector3.Distance(transform.position,target) > 0.0001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, _playerData.RollingSpeed * Time.deltaTime);
            yield return null;
        }
        _canHit = true;
        Physics.IgnoreLayerCollision(_playerLayerNum, _enemyLayerNum, false);
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

            for (int i = 0; i < _playerRend.Count; i++)
            {
                _playerRend[i].enabled = false;
            }

            yield return null;

            for (int i = 0; i < _playerRend.Count; i++)
            {
                _playerRend[i].enabled = true;
            }

            yield return null;
        }
    }
    void ApplyYVel()
    {
        if (_collideChecker.IsLanding)
        {
            OnLand?.Invoke();
        }

        transform.position += Vector3.up * _yVel * Time.fixedDeltaTime;
        if (transform.position.y < -20f)
        {
            transform.position = new Vector3(0, -1f, 0);
            StartCoroutine(GetDamageSequence(Vector3.zero));
        }
    }
public void OnDrawGizmos()
    {
        if (_col == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            _col.bounds.center,
            new Vector3(0.5f, 0.5f, 0.5f) *2
        );
        Vector3 checkBoundary = _col.bounds.center + (transform.forward);
        Gizmos.DrawWireCube(
            checkBoundary,
            new Vector3(0.5f, 0.5f, 0.5f) *2
        );
    }
}
