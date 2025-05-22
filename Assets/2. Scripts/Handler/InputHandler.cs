using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 MoveInput         { get; private set; }
    public bool    JumpRequested     { get; private set; }
    public bool    InteractRequested { get; private set; }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!PlayerController.Instance.IsGrounded)
            return;
        if (context.performed)
        {
            JumpRequested = true;
        }
    }

    public void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UIManager.Instance.CheckOpenPopup(UIInventory.Instance);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            InteractRequested = true;
        }
    }

    public void ResetJumpRequested()
    {
        JumpRequested = false;
    }

    public void ResetInteractRequested()
    {
        InteractRequested = false;
    }
}