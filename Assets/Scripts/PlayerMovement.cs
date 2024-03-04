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
        land,
        dodge,
        attack,
        stun
    }

    private Rigidbody rb;
    //public MovementState mState;

    [Header("Move")]
    [SerializeField] float horizontalMultiplier;
    [SerializeField] float verticalMultiplier;
    [SerializeField] float moveSpeed;

    private bool canMove = true;
    private bool facingRight = true;
    private Vector3 moveDirection = Vector3.zero;

    [Header("Jump")]
    [SerializeField] float jumpingPower;
    [SerializeField][Range(0, 10)] float fallSpeed;

    private Coroutine landRoutine;

    [Header("Dodge")]
    [SerializeField] float dodgeForce;
    [SerializeField] float dodgeTime;
    [SerializeField] float dodgeCooldown;

    private bool dodgeReady = true;
    private float dodgeCooldownTimer;
    private Coroutine dodgeRoutine, dodgeCooldownRoutine;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame 
    void FixedUpdate()
    {
        Movement();
        CheckDirection();
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
            if (IsJumping())
                return;

            // Invoke run/idle events depending on player velocity
            if (rb.velocity.x != 0 || rb.velocity.z != 0)
            {
                SetState(PlayerState.Run);

                if (player.IsGrounded() && CheckState(PlayerState.Run))
                    player.ID.events.OnMovement?.Invoke();

            }
            else if (rb.velocity == Vector3.zero)
            {
                SetState(PlayerState.Idle);

                if (player.IsGrounded() && CheckState(PlayerState.Idle))
                    player.ID.events.OnStationary?.Invoke();
            }
        }
    }

    private void CheckDirection()
    {
        if (canMove)
        {
            if (moveDirection.x > 0 && !facingRight)
            {
                facingRight = true;
                player.ID.events.OnFacingRight?.Invoke(facingRight);
            }
            else if (moveDirection.x < 0 && facingRight)
            {
                facingRight = false;
                player.ID.events.OnFacingRight?.Invoke(facingRight);
            }
        }
    }

    private void CheckFall()
    {
        // Check player velocity to determine what movement state they are in air
        if (!player.IsGrounded() && rb.velocity.y < 0f)
        {
            SetState(PlayerState.Fall);
            player.ID.events.OnFalling?.Invoke();

            // Add weight to character at top of jump to avoid floaty feeling
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * fallSpeed * 10f * Time.fixedDeltaTime, rb.velocity.z);
        }
        else if (rb.velocity.y == 0f && CheckState(PlayerState.Fall))
        {
            Debug.Log("Landed");
            SetState(PlayerState.Land);
            player.ID.events.OnLanding?.Invoke();
        }
    }

    private void Jump()
    {
        if (canMove)
        {
            if (player.IsGrounded())
            {
                SetState(PlayerState.Jump);
                player.ID.events.OnJumpUsed?.Invoke();
            
                rb.velocity = new Vector3(rb.velocity.x, jumpingPower, rb.velocity.z);
            }
        }
    }

    private void Dodge()
    {
        if (!dodgeReady)
        {
            Debug.Log("Dodge is in cooldown! " + dodgeCooldownTimer + " seconds left");
            return;
        }

        if (canMove)
        {            
            if (player.IsGrounded() && !IsJumping())
            {
                canMove = false;

                // Save player movement direction
                Vector3 dodgeDirection = moveDirection;

                SetState(PlayerState.Dodge);
                player.ID.events.OnDodgeUsed?.Invoke();

                // Begin dodge movement and cooldown
                if (dodgeRoutine != null)
                    StopCoroutine(dodgeRoutine);

                dodgeRoutine = StartCoroutine(DodgeRoutine(dodgeDirection));

                if (dodgeCooldownRoutine != null)
                    StopCoroutine(dodgeCooldownRoutine);

                dodgeCooldownRoutine = StartCoroutine(DodgeCooldown());
            }
        }
    }

    IEnumerator DodgeRoutine(Vector3 dodgeDirection)
    {
        if (dodgeDirection == Vector3.zero)
        {
            if (facingRight)
                rb.velocity = new Vector3(1f * dodgeForce, 0f, 0f);
            else
                rb.velocity = new Vector3(-1f * dodgeForce, 0f, 0f);
        }
        else
        {
            rb.velocity = new Vector3(
                dodgeDirection.x * dodgeForce * horizontalMultiplier, 
                0f, 
                dodgeDirection.y * dodgeForce * verticalMultiplier);
        }

        yield return new WaitForSeconds(dodgeTime);

        // Renable movement
        canMove = true;
    }

    IEnumerator DodgeCooldown()
    {
        Debug.Log("Dodge cooldown started");
        dodgeReady = false;
        dodgeCooldownTimer = dodgeCooldown;

        while (dodgeCooldownTimer > 0)
        {
            dodgeCooldownTimer -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        dodgeReady = true;
    }

    private void OnLandingDone()
    {
        SetState(PlayerState.Idle);
    }

    private bool IsJumping()
    {
        if (CheckState(PlayerState.Jump) || CheckState(PlayerState.Fall) || CheckState(PlayerState.Land))
            return true;
        else
            return false;
    }

    private void SetMovementVector(Vector2 direction)
    {
        moveDirection = direction;
    }

    #region PLAYER STATE REFERENCES

    private bool CheckState(PlayerState state)
    {
        if (player.pState.Equals(state)) return true;
        return false;
    }

    private void SetState(PlayerState state)
    {
        player.pState = state;
    }

    #endregion

    private void OnEnable()
    {
        player.ID.events.OnMoveInput += SetMovementVector;
        player.ID.events.OnJumpInput += Jump;
        player.ID.events.OnDodgeInput += Dodge;

        player.ID.events.OnLandAnimDone += OnLandingDone;
    }

    private void OnDisable()
    {
        player.ID.events.OnMoveInput -= SetMovementVector;
        player.ID.events.OnJumpInput -= Jump;
        player.ID.events.OnDodgeInput -= Dodge;

        player.ID.events.OnLandAnimDone -= OnLandingDone;
    }
}
