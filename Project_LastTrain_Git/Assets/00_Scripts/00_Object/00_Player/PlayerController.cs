using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerAction playerAction;

    public void Init()
    {
        playerAction = GetComponent<PlayerAction>();
    }
    public void OnInputUpdate()
    {
        playerAction.MoveDir = Vector3.zero;

        if (GameManager.Instance.IsPaused())
        {
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerAction.Move(Vector3.left);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerAction.Move(Vector3.right);
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            playerAction.Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            playerAction.Interaction();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerAction.Rolling();
        }
    }

}
