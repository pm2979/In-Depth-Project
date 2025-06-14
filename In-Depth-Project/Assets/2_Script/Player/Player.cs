using UnityEngine;

public class Player : MonoBehaviour
{
    [field:SerializeField] public PlayerSO Data {  get; private set; }

    [field:Header("Animations")]
    [field:SerializeField]public PlayerAnimationData AnimationData {  get; private set; }

    public Animator Animator { get; private set; }
    public PlayerController Input {  get; private set; }
    public CharacterController Controller { get; private set; }

    private PlayerStateMachine stateMachine;

    public ForceReceiver ForceReceiver { get; private set; }

    public PlayerCondition Condition { get; private set; }

    [field: SerializeField] public Weapon Weapon { get; private set; }
    [field: SerializeField] public Enemy Target { get; set; }


    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerController>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        Condition = GetComponent<PlayerCondition>();

        stateMachine = new PlayerStateMachine(this);

    }

    void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        Condition.OnDie += OnDie;
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    void OnDie()
    {
        Animator.SetTrigger("Die");
        enabled = false;
    }

}
