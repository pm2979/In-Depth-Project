using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Player.Data.GroundData;
    }

    public virtual void Enter()
    {
        AddInputActionCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionCallbacks();
    }

    protected virtual void AddInputActionCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled += OnMovementCanceled;
        input.playerActions.Run.started += OnRunStarted;
        input.playerActions.Jump.started += OnJumpStarted;
        input.playerActions.Attack.performed += OnAttackPerformed;
        input.playerActions.Attack.canceled += OnAttackCanceled;
    }

    protected virtual void RemoveInputActionCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled -= OnMovementCanceled;
        input.playerActions.Run.started -= OnRunStarted;
        input.playerActions.Jump.started -= OnJumpStarted;
        input.playerActions.Attack.performed -= OnAttackPerformed;
        input.playerActions.Attack.canceled -= OnAttackCanceled;
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {
        
    }


    public virtual void Update()
    {
        Move();
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnAttackPerformed(InputAction.CallbackContext context)
    {
        // UI 위 클릭이면 무시
        if (EventSystem.current.IsPointerOverGameObject()) return;

        stateMachine.IsAttacking = true;
    }

    protected virtual void OnAttackCanceled(InputAction.CallbackContext context)
    {
        stateMachine.IsAttacking = false;
    }

    protected void StartAnimaion(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimaion(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash, false);
    }

    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.Player.Input.playerActions.Movement.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();

        Move(movementDirection);
        Rotate(movementDirection);
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 forward = stateMachine.MainCamTransform.forward;
        Vector3 right = stateMachine.MainCamTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveTarget = forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;

        if(moveTarget == Vector3.zero && stateMachine.Player.Target != null)
        {
            Vector3 dir = (stateMachine.Player.Target.transform.position - stateMachine.Player.transform.position);
            return dir.normalized;
        }
        else
        {
            return moveTarget;
        }
    }

    private void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed();

        // 입력에 의한 포지션 변화 + ForceReceiver의 변화 값 적용
        stateMachine.Player.Controller.Move(
            ((direction * movementSpeed)
            + stateMachine.Player.ForceReceiver.Movement)
            * Time.deltaTime
        );
    }

    private float GetMovementSpeed()
    {
        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return moveSpeed;
    }

    private void Rotate(Vector3 direction)
    {
        if(direction !=  Vector3.zero)
        {
            Transform playerTransform = stateMachine.Player.transform;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    protected void ForceMove()
    {
        stateMachine.Player.Controller.Move(stateMachine.Player.ForceReceiver.Movement * Time.deltaTime);
    }


    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if(animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if(!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

    protected bool IsInAttackRange() // 공격 사거리 확인
    {
        if (stateMachine.MovementInput != Vector2.zero) return false;

        if (stateMachine.Player.Target == null) return false;

        float playerDistanceSqr = (stateMachine.Player.Target.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.Player.Data.AttackData.AttackRange * stateMachine.Player.Data.AttackData.AttackRange;
    }

}
