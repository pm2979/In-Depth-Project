using System;
using UnityEngine;

public class EnemyCondition : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100; // �ִ� ü��
    private float health; // ���� ü��
    public event Action OnDie;

    public bool IsDie = false;

    private void Start()
    {
        health = maxHealth;
        EventBus.Publish(EventType.HPUI, health / maxHealth);
    }

    public void TakeDamage(int damage) // �޴� ������
    {
        if (health == 0) return;

        health = Mathf.Max(health - damage, 0);

        if (health == 0)
        {
            IsDie = true;
            OnDie?.Invoke();
        }

        Debug.Log(health);
    }
}
