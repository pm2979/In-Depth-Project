using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    [SerializeField] private float drag = 0.3f;

    private float vertiaclVelocity;

    public Vector3 Movement => impact + Vector3.up * vertiaclVelocity;
    private Vector3 dampingVelocity;
    private Vector3 impact;

    void Start()
    {
        controller = GetComponent<CharacterController>();        
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            vertiaclVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            vertiaclVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
    }


    public void Jump(float jumpForce)
    {
        vertiaclVelocity += jumpForce;
    }

    public void Reset()
    {
        vertiaclVelocity = 0;
        impact = Vector3.zero;
    }
}
