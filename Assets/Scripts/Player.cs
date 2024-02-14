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
}
