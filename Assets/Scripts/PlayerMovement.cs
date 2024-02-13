using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : PlayerSystem
{
    private Rigidbody rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [SerializeField] float horizontalMultiplier;
    [SerializeField] float verticalMultiplier;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpingPower;

    private bool canMove = true;
    private bool isMoving = false;
    private bool isFacingRight = true;
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
        WeightedGravity();
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
                    player.ID.events.OnMovement?.Invoke();
                }
            }
            else if (rb.velocity.x == 0 || rb.velocity.z == 0)
            {
                if (isMoving)
                {
                    isMoving = false;
                    player.ID.events.OnStationary?.Invoke();
                }
            }
        }
    }

    private void WeightedGravity()
    {
        if (!IsGrounded() && rb.velocity.y < 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 1.05f, rb.velocity.z);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(groundCheck.position, Vector3.down, out hit, 0.3f, groundLayer);
    }

    public void Move(InputAction.CallbackContext context)
    {
        // TODO break this up into a playerinput script
        moveDirection = context.ReadValue<Vector2>();
        player.ID.events.OnMoveInput?.Invoke(moveDirection);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        // TODO break this up into a playerinput script
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpingPower, rb.velocity.z);
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            return;
            // If jump button is not held for full duration, player will have a shorter jump
            //rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, rb.velocity.z);
        }
    }
}
