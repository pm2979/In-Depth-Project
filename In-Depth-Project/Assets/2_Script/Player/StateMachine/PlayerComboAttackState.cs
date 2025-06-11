public class PlayerComboAttackState : PlayerAttackState
{
    private bool alreadyAppliedCombo;
    private bool alreadyApplyForce;
    private bool alreadyAppliedDealing;

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
        alreadyAppliedDealing = false;

        int comboindex = stateMachine.ComboIndex;
        attackInfoData = stateMachine.Player.Data.AttackData.GetAttackInfoData(comboindex);
        stateMachine.Player.Animator.SetInteger("Combo", comboindex);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimaion(stateMachine.Player.AnimationData.ComboAttackParameterHash);

        if (!alreadyAppliedCombo)
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
                // ÄÞº¸ ½Ãµµ
                TryComboAttack();
            }

            if(norlizedTime >= attackInfoData.ForceTransitionTime)
            {
                // ´ïÇÎ ½Ãµµ
                TryApplyForce();
            }

            if (!alreadyAppliedDealing && norlizedTime >= attackInfoData.Dealing_Start_TransitionTime)
            {
                stateMachine.Player.Weapon.SetAttack(attackInfoData.Damage, attackInfoData.Force);
                stateMachine.Player.Weapon.gameObject.SetActive(true);
                alreadyAppliedDealing = true;
            }

            if (alreadyAppliedDealing && norlizedTime >= attackInfoData.Dealing_End_TransitionTime)
            {
                stateMachine.Player.Weapon.gameObject.SetActive(false);
            }

        }
        else
        {
            if (alreadyAppliedCombo)
            {
                stateMachine.ComboIndex = attackInfoData.ComboStateIndex;
                stateMachine.ChangeState(stateMachine.ComboAttackState);
            }
            else if(!EnemyManager.Instance.ActiveEnemyNull())
            {
                stateMachine.Player.Target = null;
                stateMachine.ChangeState(stateMachine.IdleState);
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

        if(!stateMachine.IsAttacking && !IsInAttackRange()) return;
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
