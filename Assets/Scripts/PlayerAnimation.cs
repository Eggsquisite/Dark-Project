using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : PlayerSystem
{
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer sp;

    private Rigidbody rb;
    private bool isMoving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        AnimateGroundMovement();
    }

    void AnimateGroundMovement()
    {
        if (!player.IsGrounded() || player.STATE == Player.PlayerState.Stun)
            return;

        if (rb.velocity.x != 0 || rb.velocity.z != 0)
        {
            if (!isMoving)
            {
                isMoving = true;
                AnimateRun();
            }
        }
        
        if (rb.velocity.x == 0 && rb.velocity.z == 0)
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
        anim.Play(PlayerAnimStates.IDLE);
    }

    void AnimateRun()
    {
        anim.Play(PlayerAnimStates.RUN);
    }

    void AnimateJump()
    {
        anim.Play(PlayerAnimStates.JUMP);
    }

    void AnimateFall()
    {
        anim.Play(PlayerAnimStates.FALL);
    }

    void AnimateLand()
    {
        anim.Play(PlayerAnimStates.LAND);
    }

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
