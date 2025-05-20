using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StatType
{
    MaxHp,
    CurrentHp,
    MoveSpeed,
    JumpPower,
    Stamina,
}

public enum StatValueType
{
    Base,
    Buff,
    Equipment
}


public class StatManager : MonoBehaviour
{
    public readonly Dictionary<StatType, PlayerStat> playerStatDic = new Dictionary<StatType, PlayerStat>();

    private void Start()
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
            playerStatDic[(StatType)i] = StatFactory((StatType)i, 0);
        }
    }

    //임시
    private PlayerStat StatFactory(StatType type, float value)
    {
        return type switch
        {
            StatType.MaxHp     => new PlayerStat(type, 100),
            StatType.CurrentHp => new PlayerStat(type, 100),
            StatType.MoveSpeed => new PlayerStat(type, 5),
            StatType.JumpPower => new PlayerStat(type, 5),
            StatType.Stamina   => new PlayerStat(type, 100),
            _                  => null
        };
        // return new PlayerStat(type, value);
    }

    public void ApplyStatEffect(StatType statType, StatValueType valueType, float value)
    {
        switch (valueType)
        {
            case StatValueType.Base:
                playerStatDic[statType].ApplyBaseStat(value);
                break;
            case StatValueType.Buff:
                playerStatDic[statType].ApplyBuffStat(value);
                break;
            case StatValueType.Equipment:
                playerStatDic[statType].ApplyEquipmentStat(value);
                break;
        }

        // if (statType == StatType.MaxHp)
        // {
        //     playerStatDic[StatType.CurrentHp].MaxValue = playerStatDic[StatType.MaxHp].FinalValue;
        // }
        // else if (statType == StatType.CurrentHp)
        // {
        //     PlayerController.Instance.HpBarUI.UpdateFill(playerStatDic[StatType.CurrentHp].FinalValue, playerStatDic[StatType.MaxHp].FinalValue);
        // }
    }

    public void AllDecreaseStatValue(StatType statType, float value)
    {
        playerStatDic[statType].DecreaseAllValue(value);
        if (statType == StatType.MaxHp)
        {
            playerStatDic[StatType.CurrentHp].MaxValue = playerStatDic[StatType.MaxHp].FinalValue;
        }
        // else if (statType == StatType.CurrentHp)
        // {
        //     PlayerController.Instance.HpBarUI.UpdateFill(playerStatDic[StatType.CurrentHp].FinalValue, playerStatDic[StatType.MaxHp].FinalValue);
        // }
    }

    public float GetFinalValue(StatType type)
    {
        return playerStatDic[type].FinalValue;
    }
}