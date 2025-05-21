using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTickType
{
    Instant,
    OverTime
}

public enum EffectCategory
{
    Buff,
    Debuff
}

public class StatusEffectManager : MonoBehaviour
{
    private List<StatusEffect> activeEffects = new List<StatusEffect>();
    private StatManager statManager;

    void Start()
    {
        statManager = GetComponent<StatManager>();
    }

    public void ApplyEffect(StatusEffect effect)
    {
        Coroutine co = StartCoroutine(effect.Apply(this));
        effect.CoroutineRef = co;
        activeEffects.Add(effect);
    }

    public void ModifyBuffStat(StatType statType, StatModifierType modifierType, float value)
    {
        switch (modifierType)
        {
            case StatModifierType.Flat:
                statManager.ApplyStatEffect(statType, StatValueType.BuffFlat, value);
                break;
            case StatModifierType.Percent:
                statManager.ApplyStatEffect(statType, StatValueType.BuffPercent, value);
                break;
        }
    }

    public void RecoverEffect(StatType statType, float value)
    {
        statManager.Recover(statType, value);
    }

    public void ConsumeEffect(StatType statType, float value)
    {
        statManager.Consume(statType, value);
    }

    public void RemoveAllEffects()
    {
        foreach (StatusEffect effect in activeEffects)
        {
            if (effect.CoroutineRef != null)
            {
                StopCoroutine(effect.CoroutineRef);
            }

            effect.OnEffectRemoved(this);
        }

        activeEffects.Clear();
    }
}