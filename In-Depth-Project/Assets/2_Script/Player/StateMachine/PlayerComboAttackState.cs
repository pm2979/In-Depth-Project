using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboAttackState : PlayerAttackState
{
    private bool alreadyAppliedCombo;
    private bool alreadyApplyForce;

    AttackInfoData attackInfoData;

    public PlayerComboAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimaion(stateMachine.Player.AnimationData.ComboAttackParameterHash);

        alreadyAppliedCombo = false;
        alreadyApplyForce = false;

        int comboindex = stateMachine.ComboIndex;
        attackInfoData = stateMachine.Player.Data.AttackData.GetAttackInfoData(comboindex);
        stateMachine.Player.Animator.SetInteger("Combo", comboindex);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimaion(stateMachine.Player.AnimationData.ComboAttackParameterHash);

        if(!alreadyAppliedCombo)
        {
            stateMachine.ComboIndex = 0;
        }

    }

    public override void Update()
    {
        base.Update();

        ForceMove();

        float norlizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Attack");

        if(norlizedTime < 1f)
        {
            if(norlizedTime >= attackInfoData.ComboTransitionTime)
            {
                // ƒﬁ∫∏ Ω√µµ
                TryComboAttack();
            }

            if(norlizedTime >= attackInfoData.ForceTransitionTime)
            {
                // ¥Ô«Œ Ω√µµ
                TryApplyForce();
            }
        }
        else
        {
            if(alreadyAppliedCombo)
            {
                stateMachine.ComboIndex = attackInfoData.ComboStateIndex;
                stateMachine.ChangeState(stateMachine.ComboAttackState);
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
        }

    }

    private void TryComboAttack()
    {
        if(alreadyAppliedCombo) return;

        if(attackInfoData.ComboStateIndex == -1) return;

        if(!stateMachine.IsAttacking) return;

        alreadyAppliedCombo = true;
    }

    private void TryApplyForce()
    {
        if (alreadyApplyForce) return;
        alreadyApplyForce = true;

        stateMachine.Player.ForceReceiver.Reset();

        stateMachine.Player.ForceReceiver.AddForce(stateMachine.Player.transform.forward * attackInfoData.Force);
    }
}
