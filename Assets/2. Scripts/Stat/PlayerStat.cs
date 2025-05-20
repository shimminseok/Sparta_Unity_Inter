using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStat
{
    public StatType Type;
    public float BaseValue;
    public float BuffValue;
    public float EquipmentValue;

    public float MaxValue;
    public float MinValue = 0;

    public float FinalValue => Mathf.Clamp(BaseValue + BuffValue + EquipmentValue, MinValue, MaxValue);

    public PlayerStat(StatType type, float value)
    {
        Type = type;
        BaseValue = value;
    }

    public void ApplyBaseStat(float value)
    {
        BaseValue += value;
    }

    public void ApplyBuffStat(float value)
    {
        BuffValue += value;
    }

    public void ApplyEquipmentStat(float value)
    {
        EquipmentValue += value;
    }

    private float DecreaseBaseValue(float value)
    {
        value = Mathf.Abs(value);
        float decreaseAmount = Mathf.Min(BaseValue, value);
        BaseValue -= decreaseAmount;
        return value - decreaseAmount;
    }

    private float DecreaseBuffValue(float value)
    {
        value = Mathf.Abs(value);
        float decreaseAmount = Mathf.Min(BuffValue, value);
        BuffValue -= decreaseAmount;
        return value - decreaseAmount;
    }

    private float DecreaseEquipmentValue(float value)
    {
        value = Mathf.Abs(value);
        float decreaseAmount = Mathf.Min(BuffValue, value);
        BuffValue -= decreaseAmount;
        return value - decreaseAmount;
    }


    public void DecreaseAllValue(float value)
    {
        float remain = value;
        remain = DecreaseBuffValue(remain);
        remain = DecreaseEquipmentValue(remain);
        remain = DecreaseBaseValue(remain);
    }
}