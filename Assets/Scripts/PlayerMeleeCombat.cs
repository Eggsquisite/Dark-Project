using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeCombat : PlayerSystem
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MeleeEntryState()
    {

    }

    private void OnEnable()
    {
        player.ID.events.OnPrimaryAttackInput += MeleeEntryState;
    }

    private void OnDisable()
    {
        player.ID.events.OnPrimaryAttackInput -= MeleeEntryState;
    }
}
