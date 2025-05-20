using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item", order = 0)]
public class ItemSO : ScriptableObject
{
    public int Id;
    public string ItemName;
    public Sprite ItemSprite;
    public ItemType ItemType;
    public List<StatusEffectData> StatusEffects;
    public bool IsStackable = false;
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