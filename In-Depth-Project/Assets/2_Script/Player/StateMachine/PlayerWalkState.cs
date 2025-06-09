using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        base.Enter();
        StartAnimaion(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimaion(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    protected override void OnRunStarted(InputAction.CallbackContext context)
    {
        base.OnRunStarted(context);

        stateMachine.ChangeState(stateMachine.RunState);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.MovementInput == Vector2.zero && stateMachine.Player.Target == null)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }
}
