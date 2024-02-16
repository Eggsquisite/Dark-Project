using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : PlayerSystem
{
    public enum MovementState
    {
        idle,
        run,
        jump,
        fall,
        land
    }

    private Rigidbody rb;

    [SerializeField] float horizontalMultiplier;
    [SerializeField] float verticalMultiplier;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpingPower;
    [SerializeField][Range(0, 10)] float fallSpeed;

    private bool canMove = true;
    private Vector3 moveDirection = Vector3.zero;
    public MovementState mState;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame 
    void FixedUpdate()
    {
        Movement();
        CheckFall();
    }

    void Movement()
    {
        if (canMove)
        {
            // Movement is applied despite movement state
            rb.velocity = new Vector3(
                moveDirection.x * moveSpeed * horizontalMultiplier, 
                rb.velocity.y, 
                moveDirection.y * moveSpeed * verticalMultiplier);

            // Do not call run/idle events if player is jumping
            if (mState == MovementState.jump)
                return;

            // Invoke run/idle events depending on player velocity
            if (rb.velocity.x != 0 || rb.velocity.z != 0)
            {
                SetMovementState(MovementState.run);

                if (player.IsGrounded() && mState == MovementState.run)
                    player.ID.events.OnMovement?.Invoke();
                
            }
            else if (rb.velocity == Vector3.zero)
            {
                SetMovementState(MovementState.idle);

                if (player.IsGrounded() && mState == MovementState.idle)
                    player.ID.events.OnStationary?.Invoke();
            }
        }
    }

    private void CheckFall()
    {
        // Check player velocity to determine what movement state they are in air
        if (!player.IsGrounded() && rb.velocity.y < 0f)
        {
            SetMovementState(MovementState.fall);
            player.ID.events.OnFalling?.Invoke();

            // Add weight to character at top of jump to avoid floaty feeling
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * fallSpeed * 10f * Time.fixedDeltaTime, rb.velocity.z);
        }
        else if (rb.velocity.y == 0f && mState == MovementState.fall)
        {
            SetMovementState(MovementState.land);
            player.ID.events.OnLanding?.Invoke();
        }
    }

    private void Jump()
    {
        if (player.IsGrounded())
        {
            SetMovementState(MovementState.jump);
            
            rb.velocity = new Vector3(rb.velocity.x, jumpingPower, rb.velocity.z);
            player.ID.events.OnJumpUsed?.Invoke();
        }
    }

    private void SetMovementState(MovementState state)
    {
        if (state == MovementState.idle && mState != MovementState.idle)
            mState = MovementState.idle;
        else if (state == MovementState.run && mState != MovementState.run)
            mState = MovementState.run;
        else if (state == MovementState.jump && mState != MovementState.jump)
            mState = MovementState.jump;
        else if (state == MovementState.fall && mState != MovementState.fall)
            mState = MovementState.fall;
        else if (state == MovementState.land && mState != MovementState.land) 
            mState = MovementState.land;
    }

    private void SetMovementVector(Vector2 direction)
    {
        moveDirection = direction;
    }

    private void OnEnable()
    {
        player.ID.events.OnMoveInput += SetMovementVector;
        player.ID.events.OnJumpInput += Jump;
    }

    private void OnDisable()
    {
        player.ID.events.OnMoveInput -= SetMovementVector;
        player.ID.events.OnJumpInput -= Jump;
    }
}
