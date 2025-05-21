using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StatType
{
    MaxHp,
    MoveSpeed,
    JumpPower,
    MaxStamina,


    CurrentHp,
    CurrentStamina,
}

public enum StatValueType
{
    Base,
    BuffFlat,
    BuffPercent,
    Equipment
}


public class StatManager : MonoBehaviour
{
    public readonly Dictionary<StatType, StatBase> playerStat = new Dictionary<StatType, StatBase>();


    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        // for (int i = 0; i < playerData.statData.Count; i++)
        // {
        //     StatType type     = playerData.statData[i].StatType;
        //     float    value    = playerData.statData[i].Value;
        //     var      statData = StatFactory(type, value);
        //
        //     playerStatDic[type] = statData;
        // }
        //임시

        for (int i = 0; i < Enum.GetValues(typeof(StatType)).Length; i++)
        {
            playerStat[(StatType)i] = BaseStatFactory((StatType)i, 0);
        }
    }

    private StatBase BaseStatFactory(StatType type, float value)
    {
        return type switch
        {
            StatType.MaxHp      => new CalculatedStat(type, 100),
            StatType.MoveSpeed  => new CalculatedStat(type, 5),
            StatType.JumpPower  => new CalculatedStat(type, 5),
            StatType.MaxStamina => new CalculatedStat(type, 100),

///////////////////////////////////////////////////////////////////////////////////
            StatType.CurrentHp      => new ResourceStat(type, 100),
            StatType.CurrentStamina => new ResourceStat(type, 100),
            _                       => null
        };
    }

    public T GetStat<T>(StatType type) where T : StatBase
    {
        return playerStat[type] as T;
    }

    public float GetValue(StatType type)
    {
        return playerStat[type].GetCurrent();
    }

    public void Recover(StatType statType, float value)
    {
        if (playerStat[statType] is ResourceStat res)
        {
            res.Recover(value);
        }
    }

    public void Consume(StatType statType, float value)
    {
        if (playerStat[statType] is ResourceStat res)
        {
            res.Consume(value);
        }
    }

    public void ApplyStatEffect(StatType type, StatValueType valueType, float value)
    {
        if (playerStat[type] is not CalculatedStat stat) return;

        switch (valueType)
        {
            case StatValueType.Base:        stat.ModifyBaseValue(value); break;
            case StatValueType.BuffFlat:    stat.ModifyBuffFlat(value); break;
            case StatValueType.BuffPercent: stat.ModifyBuffPercent(value); break;
            case StatValueType.Equipment:   stat.ModifyEquipmentValue(value); break;
        }

        switch (type)
        {
            // 예외 처리: Max 변경 시 Current도 갱신
            case StatType.MaxHp:
                SyncCurrentWithMax(StatType.CurrentHp, stat);
                break;
            case StatType.MaxStamina:
                SyncCurrentWithMax(StatType.CurrentStamina, stat);
                break;
        }
    }

    private void SyncCurrentWithMax(StatType curStatType, CalculatedStat stat)
    {
        if (playerStat.TryGetValue(curStatType, out var res) && res is ResourceStat curStat)
        {
            curStat.SetMax(stat.FinalValue);
        }
    }
}