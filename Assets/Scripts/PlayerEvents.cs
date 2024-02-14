using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerEvents
{
    // Input Events

    public Action<Vector2> OnMoveInput;

    public Action OnJumpInput;
    public Action OnDodgeInput;

    public Action OnPrimaryAttackInput;
    public Action OnSecondaryAttackInput;

    // UI Events

    public Action<int> OnHealthChanged;
    public Action<int> OnPowerChanged;

    // Movement Events

    public Action<Vector2> OnMovement;
    public Action OnStationary;
    public Action OnFalling;
    public Action OnLanding;

    public Action OnJumpUsed;
    public Action OnDodgeUsed;

    public Action OnPrimaryAttackUsed;
    public Action OnSecondaryAttackUsed;

    // Animation Events

    public Action OnIdleAnim;
    public Action OnWalkAnim;

    // Collision Events

    public Action<int> OnTakeDamage;

    // Health Events

    public Action OnDeath;

    // Status Effect Events
}
