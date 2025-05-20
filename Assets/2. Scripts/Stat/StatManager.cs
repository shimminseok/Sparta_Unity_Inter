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
            _                  => null
        };
        // return new PlayerStat(type, value);
    }


    public float GetFinalValue(StatType type)
    {
        return playerStatDic[type].FinalValue;
    }
}