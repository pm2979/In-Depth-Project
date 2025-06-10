using System;
using UnityEngine;

[Serializable]
public class Condition
{
    public float maxValue;
    public float curValue;

    public void Add(float value) // + °ª
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Subtract(float value) // - °ª
    {
        curValue = Mathf.Max(curValue - value, 0f);
    }

    public float PercentValue()
    {
        return curValue / maxValue;
    }
}

public enum ConditionType
{
    Hp,
    EXP
}
