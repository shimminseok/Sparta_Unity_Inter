using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsumeItemData", menuName = "Item/Consume", order = 0)]
public class ConsumeItemSO : ItemSO
{
    public List<StatusEffectData> StatusEffects;
    public float CoolTime;
}