using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimaion(stateMachine.Player.AnimationData.GroundParameterHash);

        if (EnemyManager.Instance.GetNearestEnemy(stateMachine.Player.transform.position) != null)
        {
            stateMachine.Player.Target = EnemyManager.Instance.GetNearestEnemy(stateMachine.Player.transform.position);
        }
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimaion(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if(stateMachine.IsAttacking || IsInAttackRange())
        {
            OnAttack();
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if(!stateMachine.Player.Controller.isGrounded && stateMachine.Player.Controller.velocity.y < Physics.gravity.y * Time.deltaTime ) 
        { 
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.MovementInput == Vector2.zero) return;

        if(EnemyManager.Instance.GetNearestEnemy(stateMachine.Player.transform.position) != null)
        {
            stateMachine.Player.Target = EnemyManager.Instance.GetNearestEnemy(stateMachine.Player.transform.position);
        }
        else
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }

        base.OnMovementCanceled(context);
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        stateMachine.ChangeState(stateMachine.JumpState);
    }

    protected virtual void OnAttack()
    {
        stateMachine.ChangeState(stateMachine.ComboAttackState);
    }

    protected bool IsInAttackRange()
    {
        if (stateMachine.MovementInput != Vector2.zero) return false;

        if (stateMachine.Player.Target == null) return false;

        float playerDistanceSqr = (stateMachine.Player.Target.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.Player.Data.AttackData.AttackRange * stateMachine.Player.Data.AttackData.AttackRange;
    }
}
