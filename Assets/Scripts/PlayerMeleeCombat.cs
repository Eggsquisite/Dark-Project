using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeCombat : PlayerSystem
{

    private bool isAttacking = false;
    private bool shouldCombo = false;
    private int attackIndex = 0;
    private int attackIndexMax = 2;
    private float attackTimer = 0f;
    private float attackDuration;

    private int attackType;

    [SerializeField] private float attackTimerBuffer;
    [SerializeField] private List<int> damage;

    private void Update()
    {
        if (attackTimer >= 0f) attackTimer -= Time.deltaTime;
        if (attackDuration >= 0f) attackDuration -= Time.deltaTime;

        AttackCombo();
    }

    private void AttackCombo()
    {
        // If player is attacking
        if (isAttacking)
        {
            if (attackDuration <= 0f)
            {
                if (shouldCombo && attackIndex < attackIndexMax)
                {
                    NextAttackState();
                }
                else
                {
                    ResetAttackState();
                }
            }
        }
    }

    private void MeleeEntryState(int type)
    {
        // attackType == 1 if it is primary attack, 2 if secondary attack
        attackType = type;

        // If player has not started attacking, begin attack combo, they must wait the minimum time for the next attack to continue
        if (!isAttacking)
            NextAttackState();
    }

    private void NextAttackState()
    {
        attackIndex += 1;
        isAttacking = true;
        attackTimer = attackTimerBuffer;

        // Play grounded attacks
        if (player.IsGrounded())
        {
            if (attackType.Equals(1))
                player.ID.events.OnPrimaryGroundAttackUsed?.Invoke(attackIndex);
            else if (attackType.Equals(2))
                player.ID.events.OnSecondaryGroundAttackUsed?.Invoke(attackIndex);
        }
        // Play air attacks
        else
        {
            if (attackType.Equals(1))
                player.ID.events.OnPrimaryAirAttackUsed?.Invoke(attackIndex);
            else if (attackType.Equals(2))
                player.ID.events.OnSecondaryAirAttackUsed?.Invoke(attackIndex);
        }
    }

    private void ResetAttackState()
    {
        attackIndex = 0;
        shouldCombo = false;
        isAttacking = false;
    }

    private void AttackShouldCombo(float attackWindowOpen)
    {
        if (attackWindowOpen > 0f && attackTimer > 0f)
            shouldCombo = true; 
    }

    private void SetAttackDuration(float duration)
    {
        attackDuration = duration;
    }

    private void Attack()
    {
        // Do dmg based off attackIndex, which states its damage
    }

    private void OnEnable()
    {
        player.ID.events.OnAttackInput += MeleeEntryState;
        player.ID.events.OnAttackWeaponActive += Attack;
        player.ID.events.OnAttackWindowOpen += AttackShouldCombo;

        player.ID.events.OnAttackAnimationDuration += SetAttackDuration;
    }

    private void OnDisable()
    {
        player.ID.events.OnAttackInput -= MeleeEntryState;
        player.ID.events.OnAttackWeaponActive -= Attack;
        player.ID.events.OnAttackWindowOpen -= AttackShouldCombo;

        player.ID.events.OnAttackAnimationDuration -= SetAttackDuration;
    }
}
