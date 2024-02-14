using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : PlayerSystem
{
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer sp;

    private bool isMoving = false;

    void CheckMovement(Vector2 moveSpeed)
    {
        if (moveSpeed.x != 0 || moveSpeed.y != 0)
        {
            if (!isMoving)
            {
                isMoving = true;
                AnimateRun();
            }
        }
        else if (moveSpeed.x == 0 || moveSpeed.y == 0)
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
        anim.Play("Idle");
    }

    void AnimateRun()
    {
        anim.Play("Run");
    }

    void AnimateJump()
    {
        anim.Play("Jump");
    }

    void AnimateFall()
    {
        anim.Play("Fall");
    }

    private void OnEnable()
    {
        player.ID.events.OnMoveInput += CheckMovement;
        player.ID.events.OnMoveInput += CheckDirection;

        player.ID.events.OnFalling += AnimateFall;
        //player.ID.events.OnStationary += AnimateIdle;
        //player.ID.events.OnMovement += AnimateRun;
    }

    private void OnDisable()
    {
        player.ID.events.OnMoveInput -= CheckMovement;
        player.ID.events.OnMoveInput -= CheckDirection;

        player.ID.events.OnFalling -= AnimateFall;
        //player.ID.events.OnStationary -= AnimateIdle;
        //player.ID.events.OnMovement -= AnimateRun;
    }
}
