public class PlayerCondition : BaseCondition
{
    protected override void Start()
    {
        base.Start();
        EventBus.Publish(EventType.HpUI, HPCondition.PercentValue());
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
        EventBus.Publish(EventType.HpUI, HPCondition.PercentValue());
    }

    public void Heal(float amount)
    {
        HPCondition.Add(amount);
        EventBus.Publish(EventType.HpUI, HPCondition.PercentValue());
    }
}