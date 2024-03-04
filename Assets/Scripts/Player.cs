using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    // Player states to manage allowed behaviors
    Idle,
    Run,
    Jump,
    Fall,
    Land,
    Dodge,
    Hit,
    Death,
    Stun,
    Attack
}

public class Player : MonoBehaviour
{

    public PlayerID ID;
    [SerializeField] private PlayerState _playerState = PlayerState.Idle;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    public PlayerState pState 
    {
        set
        {
            if (_playerState == value)
            {
                return;
            }

            _playerState = value;


        }
        get { return _playerState; }
    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(groundCheck.position, Vector3.down, out hit, 0.3f, groundLayer);
    }
}
