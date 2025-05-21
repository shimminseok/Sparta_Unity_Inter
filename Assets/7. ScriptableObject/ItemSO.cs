using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Consume,
    Equipment,
    Material,
}

public class ItemSO : ScriptableObject
{
    public int Id;
    public string ItemName;
    public Sprite ItemSprite;
    public bool IsStackable = false;
    public ItemType ItemType;
}