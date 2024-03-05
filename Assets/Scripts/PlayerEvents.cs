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

    public Action<int> OnAttackInput;

    // UI Events

    public Action<int> OnHealthChanged;
    public Action<int> OnPowerChanged;

    // Movement Events

    public Action OnMovement;
    public Action OnStationary;
    public Action OnFalling;
    public Action OnLanding;
    public Action<bool> OnFacingRight;

    public Action OnJumping;
    public Action OnDodging;

    // Combat Events

    // param takes attackIndex to determine what animation to play
    public Action<int> OnPrimaryGroundAttackUsed;
    public Action<int> OnSecondaryGroundAttackUsed;

    public Action<int> OnPrimaryAirAttackUsed;
    public Action<int> OnSecondaryAirAttackUsed;

    // Animation Events

    public Action OnIdleAnim;
    public Action OnWalkAnim;
    public Action OnLandingDone;

    public Action OnAttackAnimStart;
    public Action OnAttackAnimDone;

    public Action OnDodgeAnimStart;
    public Action OnDodgeAnimDone;

    public Action OnAttackWeaponActive;
    public Action<float> OnAttackWindowOpen;
    public Action<float> OnAttackAnimationDuration;

    // Collision Events

    public Action<int> OnTakeDamage;

    // Health Events

    public Action OnDeath;
    //internal Action<int> OnPrimaryAttackUsed;

    // Status Effect Events
}
