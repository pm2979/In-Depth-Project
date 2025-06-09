using System;
using UnityEngine;

public class EnemyCondition : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100; // 최대 체력
    private float health; // 현제 체력
    public event Action OnDie;

    public bool IsDie = false;

    private void Start()
    {
        health = maxHealth;
        EventBus.Publish(EventType.HPUI, health / maxHealth);
    }

    public void TakeDamage(int damage) // 받는 데미지
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
