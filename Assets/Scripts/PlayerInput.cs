using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInput : PlayerSystem
{
    private Vector2 moveDirection;

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        player.ID.events.OnMoveInput?.Invoke(moveDirection);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.ID.events.OnJumpInput?.Invoke();
        }
    }

    public void Dodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.ID.events.OnDodgeInput?.Invoke();
        }
    }

    public void PrimaryAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.ID.events.OnAttackInput?.Invoke(1);
        }
    }

    public void SecondaryAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.ID.events.OnAttackInput?.Invoke(2);
        }
    }
}
