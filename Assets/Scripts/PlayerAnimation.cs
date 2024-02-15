using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : PlayerSystem
{
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer sp;
    private RuntimeAnimatorController ac;

    private Rigidbody rb;
    private bool isMoving = false;
    private string currentState;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ac = anim.runtimeAnimatorController;
    }

    private void Update()
    {
        if (player.IsGrounded())
        {
            AnimateMovement();
            AnimateStationary();
        }
    }

    void AnimateMovement()
    {
        if (rb.velocity != Vector3.zero)
        {
            if (!isMoving)
            {
                isMoving = true;
                AnimateRun();
            }
        }
        
    }

    void AnimateStationary()
    {    
        if (rb.velocity == Vector3.zero)
        {
            if (isMoving)
            {
                isMoving = false;
                AnimateIdle();
            }
        }   
    }

    void CheckDirection(Vector2 moveDirection)
    {
        if (moveDirection.x > 0) sp.flipX = false;
        else if (moveDirection.x < 0) sp.flipX = true;
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
        player.ID.events.OnMoveInput += CheckDirection;

        //player.ID.events.OnStationary += AnimateIdle;
        //player.ID.events.OnMovement += AnimateRun;

        player.ID.events.OnJumpUsed += AnimateJump;
        player.ID.events.OnFalling += AnimateFall;
        player.ID.events.OnLanding += AnimateLand;
    }

    private void OnDisable()
    {
        player.ID.events.OnMoveInput -= CheckDirection;

        //player.ID.events.OnStationary -= AnimateIdle;
        //player.ID.events.OnMovement -= AnimateRun;

        player.ID.events.OnJumpUsed -= AnimateJump;
        player.ID.events.OnFalling -= AnimateFall;
        player.ID.events.OnLanding -= AnimateLand;
    }
}
