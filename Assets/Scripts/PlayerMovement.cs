using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : PlayerSystem
{
    private Rigidbody rb;

    [SerializeField] float horizontalMultiplier;
    [SerializeField] float verticalMultiplier;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpingPower;

    private bool canMove = true;
    private bool isMoving = false;
    private bool isFalling = false;
    private Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyMovement();
        CheckFall();
    }

    void ApplyMovement()
    {
        if (canMove)
        {
            // Move and call the walk animation event
            rb.velocity = new Vector3(
                moveDirection.x * moveSpeed * horizontalMultiplier, 
                rb.velocity.y, 
                moveDirection.y * moveSpeed * verticalMultiplier);

            if (rb.velocity.x != 0 || rb.velocity.z != 0)
            {
                if (!isMoving)
                {
                    isMoving = true;
                    //player.ID.events.OnMovement?.Invoke(moveDirection);
                }
            }
            else if (rb.velocity.x == 0 || rb.velocity.z == 0)
            {
                if (isMoving)
                {
                    isMoving = false;
                    //player.ID.events.OnStationary?.Invoke();
                }
            }
        }
    }

    private void CheckFall()
    {
        if (!player.IsGrounded() && rb.velocity.y < 0f)
        {
            // Add weight to character at top of jump to avoid floaty feeling
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 1.10f, rb.velocity.z);

            if (!isFalling)
            {
                isFalling = true;
                player.ID.events.OnFalling?.Invoke();
            }
        }
        else if (rb.velocity.y == 0f)
        {
            if (isFalling)
            {
                isFalling = false;
                player.ID.events.OnLanding?.Invoke();
            }
        }
    }

    private void SetMovementVector(Vector2 direction)
    {
        moveDirection = direction;
    }

    private void Jump()
    {
        if (player.IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpingPower, rb.velocity.z);
            player.ID.events.OnJumpUsed?.Invoke();
        }
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
