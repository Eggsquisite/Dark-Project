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

    #region ANIMATION EVENTS

    void InvokeLandingDone()
    {
        //player.pState = PlayerState.Idle;
        player.ID.events.OnLandingDone?.Invoke();
    }

    void InvokeAttackStart()
    {
        player.ID.events.OnAttackAnimStart?.Invoke();
    }

    void InvokeAttackEnd()
    {
        player.ID.events.OnAttackAnimDone?.Invoke();
    }

    #endregion

    #region PLAYANIMATION CALLS

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

    void AnimateAttack()
    {

    }

    #endregion

    #region ANIMATION HELPER FUNCTIONS

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

    #endregion

    private void OnEnable()
    {
        player.ID.events.OnFacingRight += CheckDirection;

        player.ID.events.OnMovement += AnimateRun;
        player.ID.events.OnStationary += AnimateIdle;
        player.ID.events.OnDodging += AnimateDodge;

        player.ID.events.OnJumping += AnimateJump;
        player.ID.events.OnFalling += AnimateFall;
        player.ID.events.OnLanding += AnimateLand;
    }

    private void OnDisable()
    {
        player.ID.events.OnFacingRight -= CheckDirection;

        player.ID.events.OnMovement -= AnimateRun;
        player.ID.events.OnStationary -= AnimateIdle;
        player.ID.events.OnDodging -= AnimateDodge;

        player.ID.events.OnJumping -= AnimateJump;
        player.ID.events.OnFalling -= AnimateFall;
        player.ID.events.OnLanding -= AnimateLand;
    }
}
