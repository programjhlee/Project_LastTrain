using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerData _playerData;
    CollideChecker _CollideChecker;
    UIHUDController _HUDController;
    PlayerAction playerAction;
    PlayerController playerController;
    PlayerAnim _playerAnim;
    void Awake()
    {

        _playerData.Level = 1;
        _playerData.AttackPower = 5;
        _playerData.FixPower = 2;
        _playerData.MoveSpeed = 5;
        _playerData.JumpForce = 15;
        _playerData.RollingSpeed = 15;

        _CollideChecker = GetComponent<CollideChecker>();
        playerAction = GetComponent<PlayerAction>();
        playerController = GetComponent<PlayerController>();
        _playerAnim = GetComponent<PlayerAnim>();
        _HUDController = GetComponent<UIHUDController>();
        
        playerAction.Init(_playerData);
        playerController.Init();
        _playerAnim.Init();
        _HUDController.Init();
    }

    void Update()
    {
        playerController.OnInputUpdate();
        _playerAnim.OnUpdate();
        _CollideChecker.LandCheck();
    }
    void FixedUpdate()
    {
        if (GameManager.Instance.IsPaused()) return;
        playerAction.ProcessMovement();
    }
}
