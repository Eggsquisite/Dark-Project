using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : PlayerSystem
{
    private float horizontalMove, verticalMove;

    // Start is called before the first frame update
    void Start()
    {
        
        Debug.Log(player.ID.playerName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
