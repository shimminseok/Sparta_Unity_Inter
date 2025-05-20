public static class BuffFactory
{
    public static StatusEffect CreateBuff(StatusEffectData data)
    {
        StatusEffect effect = data.EffectType switch
        {
            StatusEffectType.InstantBuff       => new InstantBuff(),
            StatusEffectType.OverTimeBuff      => new OverTimeBuff(),
            StatusEffectType.InstantDebuff     => new InstantDebuff(),
            StatusEffectType.OverTimeDebuff    => new OverTimeDebuff(),
            StatusEffectType.TimedModifierBuff => new TimedModifierBuff(),
            // StatusEffectType.PeriodicDamageDebuff => new PeriodicDamageDebuff
            // {
            //     Damage = new DamageInfo
            //     {
            //         Amount = data.value,
            //         Type = data.damageType
            //     }
            // },
            _ => null
        };
        if (effect != null)
        {
            effect.StatType = data.Stat.Type;
            effect.Duration = data.Duration;
            effect.Value = data.Stat.Value;
            effect.TickInterval = data.TickInterval;
            return effect;
        }

        return null;
    }
}