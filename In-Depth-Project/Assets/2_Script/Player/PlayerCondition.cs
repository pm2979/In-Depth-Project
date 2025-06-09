using System;
using UnityEngine;

public class PlayerCondition : BaseCondition
{
    protected override void Start()
    {
        base.Start();
        EventBus.Publish(EventType.HPUI, HPCondition.curValue / HPCondition.maxValue);
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
        EventBus.Publish(EventType.HPUI, HPCondition.curValue / HPCondition.maxValue);
    }
}