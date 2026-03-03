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
    void Awake()
    {
        _CollideChecker = GetComponent<CollideChecker>();
        playerAction = GetComponent<PlayerAction>();
        playerController = GetComponent<PlayerController>();
        _HUDController = GetComponent<UIHUDController>();
        
        playerAction.Init(_playerData);
        playerController.Init();
        _HUDController.Init();
    }

    void Update()
    {
        playerController.OnInputUpdate();
        _CollideChecker.LandCheck();
    }
    void FixedUpdate()
    {
        if (GameManager.Instance.IsPaused()) return;
        playerAction.ProcessMovement();
    }
}
