using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerData _playerData;
    CollideChecker _CollideChecker;
    PlayerUIController _playerUIController;
    UIHUDController _HUDController;
    PlayerAction playerAction;
    PlayerController playerController;
    PlayerAnim _playerAnim;
    void Awake()
    {
        _CollideChecker = GetComponent<CollideChecker>();
        playerAction = GetComponent<PlayerAction>();
        playerController = GetComponent<PlayerController>();
        _playerAnim = GetComponent<PlayerAnim>();
        _playerUIController = GetComponent<PlayerUIController>();
        _HUDController = GetComponentInChildren<UIHUDController>();


        playerAction.Init(_playerData);
        playerController.Init();
        _playerAnim.Init();
        _HUDController.Init();
        _playerUIController.Init();

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
    public void LateUpdate()
    {
        _playerUIController.UIUpdate();
    }
}
