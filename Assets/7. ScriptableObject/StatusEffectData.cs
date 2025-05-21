using System;
using UnityEngine;


public enum StatusEffectType
{
    InstantBuff,
    OverTimeBuff,
    InstantDebuff,
    OverTimeDebuff,
    TimedModifierBuff,
    PeriodicDamageDebuff,
    Recover,
}

public enum StatModifierType
{
    Flat,
    Percent
}

[Serializable]
public class StatusEffectData
{
    public StatusEffectType EffectType;
    public StatData Stat;
    public float Duration;
    public float TickInterval;
}

[Serializable]
public class StatData
{
    public StatType Type;
    public StatModifierType ModifierType;
    public float Value;
}