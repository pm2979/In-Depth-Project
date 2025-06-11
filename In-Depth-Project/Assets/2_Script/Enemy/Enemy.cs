using UnityEngine;

public class Enemy : MonoBehaviour
{
    [field: Header("Reference")]
    [field: SerializeField] public EnemySO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }

    private EnemyStateMachine stateMachine;

    public EnemyCondition Condition { get; private set; }

    [field: SerializeField] public Weapon Weapon { get; private set; }

    public bool IsDie { get; private set; } = false;

    private void Awake()
    {
        AnimationData.Initialize();

        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        Condition = GetComponent<EnemyCondition>();
        stateMachine = new EnemyStateMachine(this);
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        Condition.OnDie += OnDie;
    }

    private void Update()
    {
        if(!IsDie)
            stateMachine.Update();
    }

    private void FixedUpdate()
    {
        if (!IsDie)
            stateMachine.PhysicsUpdate();
    }

    void OnDie()
    {
        Animator.SetTrigger("Die");

        EnemyManager.Instance.RemoveEnemyOnDeath(this);
        IsDie = true;
        Controller.enabled = false;
        Invoke("Destroy", 5);
        
        GameManager.Instance.CurrencyManager.Add(Data.EnemyDropData.GetRandomCoin());
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }


}
