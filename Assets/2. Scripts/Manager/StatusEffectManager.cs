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

    public void ModifyStat(StatType statType, float value)
    {
        statManager.ApplyStatEffect(statType, StatValueType.Buff, value);
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

