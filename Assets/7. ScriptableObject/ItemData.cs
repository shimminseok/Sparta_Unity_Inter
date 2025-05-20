using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item", order = 0)]
public class ItemData : ScriptableObject
{
    public int Id;
    public string ItemName;
    public ItemType ItemType;
    public int Quantity;
    public List<ItemEffect> Effects;
    public float Duration = 0;
    public bool IsStackable = false;

    public ItemData DeepCopy()
    {
        return (ItemData)MemberwiseClone();
    }
}

public enum ItemType
{
    Consume,
    Equipment,
    Material,
}

public enum ModifierType
{
    Additive,
    Multiplicative,
}

[Serializable]
public class ItemEffect
{
    public StatType Type;
    public float Value;
}