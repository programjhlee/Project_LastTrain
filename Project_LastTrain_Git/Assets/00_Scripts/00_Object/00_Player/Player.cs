using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerData _playerData;
    LandChecker _landChecker;
    PlayerAction playerAction;
    PlayerController playerController;
    void Start()
    {
        _landChecker = GetComponent<LandChecker>();
        playerAction = GetComponent<PlayerAction>();
        playerController = GetComponent<PlayerController>();
        
        playerAction.Init(_playerData);
        playerController.Init();
    }

    void Update()
    {
        playerController.OnInputUpdate();
        _landChecker.LandCheck();
    }
    void FixedUpdate()
    {
        if (GameManager.Instance.IsPaused()) return;
        playerAction.ProcessMovement();
    }
}
