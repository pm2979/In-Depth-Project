using System;
using UnityEngine;

public class BaseCondition : MonoBehaviour, ITakeDamage
{
    [SerializeField] protected Condition HPCondition;
    public event Action OnDie;

    public bool IsDie = false;

    protected virtual void Start()
    {
        HPCondition.curValue = HPCondition.maxValue;
    }

    public virtual void TakeDamage(float dmg) // 받는 데미지
    {
        if (HPCondition.curValue == 0) return;

        HPCondition.Subtract(dmg);

        if (HPCondition.curValue == 0)
        {
            IsDie = true;
            OnDie?.Invoke();
        }
    }
}

public interface ITakeDamage
{
    public void TakeDamage(float dmg);
}
