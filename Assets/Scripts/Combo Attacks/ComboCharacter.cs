using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCharacter : PlayerSystem
{
    private StateMachine meleeStateMachine;

    [SerializeField] public Collider2D hitbox;
    [SerializeField] public GameObject Hiteffect;

    // Start is called before the first frame update
    void Start()
    {
        meleeStateMachine = GetComponent<StateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && meleeStateMachine.CurrentState.GetType() == typeof(IdleCombatState))
        {
            return;
            //meleeStateMachine.SetNextState(new MeleeEntryState());
        }
    }

    public void BeginMeleeEntryState(int type)
    {
        if (player.pState == PlayerState.Stun)
            return;

        if (meleeStateMachine.CurrentState.GetType() == typeof (IdleCombatState))
        {
            player.pState = PlayerState.Attack;
            meleeStateMachine.SetNextState(new MeleeEntryState());
        }
    }

    private void OnEnable()
    {
        player.ID.events.OnAttackInput += BeginMeleeEntryState;
    }

    private void OnDisable()
    {
        player.ID.events.OnAttackInput -= BeginMeleeEntryState;
    }

}
