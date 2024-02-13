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
    private bool isFacingRight = true;
    private Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMovement();
        FlipDirection();
        Debug.Log(IsGrounded());
    }

    void ApplyMovement()
    {
        if (canMove)
        {
            rb.velocity = new Vector3(
                moveDirection.x * moveSpeed * horizontalMultiplier, 
                rb.velocity.y, 
                moveDirection.y * moveSpeed * verticalMultiplier);
        }
    }

    void FlipDirection()
    {
        return;
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(groundCheck.position, Vector3.down, out hit, 0.3f, groundLayer);
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
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
