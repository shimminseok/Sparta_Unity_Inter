using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStat
{
    public StatType Type;
    public float BaseValue;
    public float BuffValue;
    public float EquipmentValue;

    public float FinalValue => BaseValue + BuffValue + EquipmentValue;

    public PlayerStat(StatType type, float value)
    {
        Type = type;
        BaseValue = value;
    }
}