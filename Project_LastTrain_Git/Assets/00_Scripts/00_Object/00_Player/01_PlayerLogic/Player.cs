using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerData _playerData;
    UIHUDController _HUDController;
    PlayerAction _playerAction;
    PlayerAnimationController _playerAnimationController;
    PlayerSoundController _playerSoundController;
    PlayerUIController _playerUIController;
    PlayerController _playerController;
    void Awake()
    {
        _playerAction = GetComponent<PlayerAction>();
        _playerController = GetComponent<PlayerController>();
        _playerAnimationController = GetComponentInChildren<PlayerAnimationController>();
        _playerUIController = GetComponent<PlayerUIController>();
        _playerSoundController = GetComponentInChildren<PlayerSoundController>();
        _HUDController = GetComponent<UIHUDController>();

        _playerAction.Init(_playerData);
        _playerController.Init();
        _playerAnimationController.Init();
        _playerSoundController.Init();
        _playerUIController.Init();
        _HUDController.Init();
    }

    void Update()
    {
        _playerController.OnInputUpdate();
        _playerAnimationController.OnAnimationUpdate();
    }
    void FixedUpdate()
    {
        _playerAction.ProcessMovement();
        _playerUIController.UIUpdate();
    }
}
