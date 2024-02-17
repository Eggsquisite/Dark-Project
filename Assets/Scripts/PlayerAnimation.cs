using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : PlayerSystem
{
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer sp;
    private RuntimeAnimatorController ac;

    private string currentState;

    private void Start()
    {
        ac = anim.runtimeAnimatorController;
    }

    void CheckDirection(bool facingRight)
    {
        if (facingRight) sp.flipX = false;
        else if (!facingRight) sp.flipX = true;
    }

    void InvokeLandingDone()
    {
        player.ID.events.OnLandAnimDone?.Invoke();
    }

    void AnimateIdle()
    {
        PlayAnimation(PlayerAnimStates.IDLE);
    }

    void AnimateRun()
    {
        PlayAnimation(PlayerAnimStates.RUN);
    }

    void AnimateJump()
    {
        PlayAnimation(PlayerAnimStates.JUMP);
    }

    void AnimateFall()
    {
        PlayAnimation(PlayerAnimStates.FALL);
    }

    void AnimateLand()
    {
        PlayAnimation(PlayerAnimStates.LAND);
    }

    void AnimateDodge()
    {
        PlayAnimation(PlayerAnimStates.DODGE);
    }

    // Animation Helper Functions ////////////////////////////////////////
    private void PlayAnimation(string newAnim)
    {
        AnimHelper.ChangeAnimationState(anim, ref currentState, newAnim);
    }

    public void ReplayAnimation(string newAnim)
    {
        AnimHelper.ReplayAnimation(anim, ref currentState, newAnim);
    }

    private float GetAnimationLength(string newAnim)
    {
        return AnimHelper.GetAnimClipLength(ac, newAnim);
    }
    // Animation Helper Functions ////////////////////////////////////////

    private void OnEnable()
    {
        player.ID.events.OnFacingRight += CheckDirection;

        player.ID.events.OnMovement += AnimateRun;
        player.ID.events.OnStationary += AnimateIdle;
        player.ID.events.OnDodgeUsed += AnimateDodge;

        player.ID.events.OnJumpUsed += AnimateJump;
        player.ID.events.OnFalling += AnimateFall;
        player.ID.events.OnLanding += AnimateLand;
    }

    private void OnDisable()
    {
        player.ID.events.OnFacingRight -= CheckDirection;

        player.ID.events.OnMovement -= AnimateRun;
        player.ID.events.OnStationary -= AnimateIdle;
        player.ID.events.OnDodgeUsed -= AnimateDodge;

        player.ID.events.OnJumpUsed -= AnimateJump;
        player.ID.events.OnFalling -= AnimateFall;
        player.ID.events.OnLanding -= AnimateLand;
    }
}
