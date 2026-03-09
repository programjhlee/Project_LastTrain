using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerData _playerData;
    CollideChecker _CollideChecker;
    UIHUDController _HUDController;
    PlayerUIController _playerUIController;
    PlayerAction playerAction;
    PlayerController playerController;
    PlayerAnim _playerAnim;
    PlayerSoundController _playerSoundController;
    void Awake()
    {
        _CollideChecker = GetComponent<CollideChecker>();
        playerAction = GetComponent<PlayerAction>();
        playerController = GetComponent<PlayerController>();
        _playerAnim = GetComponentInChildren<PlayerAnim>();
        _playerUIController = GetComponent<PlayerUIController>();
        _playerSoundController = GetComponentInChildren<PlayerSoundController>();
        _HUDController = GetComponent<UIHUDController>();

        playerAction.Init(_playerData);
        playerController.Init();
        _playerAnim.Init();
        _playerSoundController.Init();
        _playerUIController.Init();
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
        _playerUIController.UIUpdate();
        
    }
}
