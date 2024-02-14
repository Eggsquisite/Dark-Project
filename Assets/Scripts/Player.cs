using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerState
    {
        // Player states to manage allowed behaviors
        Idle,
        Run,
        Dodge,
        Hit,
        Death,
        Stun
    }

    public PlayerID ID;
    public PlayerState STATE;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    public bool IsGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(groundCheck.position, Vector3.down, out hit, 0.3f, groundLayer);
    }
}
