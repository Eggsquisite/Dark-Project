using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerEvents
{
    // Input Events

    public Action OnMoveInput;

    public Action OnJumpInput;
    public Action OnDodgeInput;

    public Action OnPrimaryAttackInput;
    public Action OnSecondaryAttackInput;

    // UI Events

    public Action<int> OnHealthChanged;
    public Action<int> OnPowerChanged;

    // Movement Events

    public Action OnMoveUsed;

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
