using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 MoveInput     { get; private set; }
    public bool    JumpPressed   { get; private set; }
    public bool    JumpRequested { get; private set; }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpPressed = true;
            JumpRequested = true;
        }
        else if (context.canceled)
        {
            JumpPressed = false;
        }
    }

    public void ResetJumpRequested()
    {
        JumpRequested = false;
    }
}