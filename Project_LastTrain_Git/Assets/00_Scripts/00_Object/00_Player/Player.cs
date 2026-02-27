using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerData _playerData;
    CollideChecker _CollideChecker;
    PlayerAction playerAction;
    PlayerController playerController;
    void Start()
    {
        _CollideChecker = GetComponent<CollideChecker>();
        playerAction = GetComponent<PlayerAction>();
        playerController = GetComponent<PlayerController>();
        
        playerAction.Init(_playerData);
        playerController.Init();
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
