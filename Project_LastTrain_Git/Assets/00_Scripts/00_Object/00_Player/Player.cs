using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    LandChecker landChecker;
    PlayerAction playerAction;
    PlayerController playerController;
    void Start()
    {
        landChecker = GetComponent<LandChecker>();
        playerAction = GetComponent<PlayerAction>();
        playerController = GetComponent<PlayerController>();
        
        playerAction.Init();
        playerController.Init();
    }

    void Update()
    {
        playerController.OnInputUpdate();
        landChecker.LandCheck();
    }
    void FixedUpdate()
    {
        if (GameManager.Instance.IsPaused()) return;
        playerAction.ProcessMovement();
    }
}
