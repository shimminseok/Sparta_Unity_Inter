using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Armor,
    Boots,
    Groves
}

public class EquipmentItem : InventoryItem
{
    public bool IsEquipped;

    public new EquipmentItemSO ItemSo => base.ItemSo as EquipmentItemSO;

    public EquipmentItem(EquipmentItemSO itemSo) : base(itemSo, 1)
    {
        IsEquipped = false;
    }
}

public class EquipmentManager : Singleton<EquipmentManager>
{
    public Dictionary<EquipmentType, EquipmentItem> EquipmentItems { get; private set; } =
        new Dictionary<EquipmentType, EquipmentItem>();


    public event Action<EquipmentType> OnEquipmentChanged;


    public void EquipItem(EquipmentItem data)
    {
        EquipmentType type = data.ItemSo.EquipmentType;

        if (EquipmentItems.ContainsKey(type))
        {
            UnEquipItem(type);
        }

        EquipmentItems[type] = data;
        foreach (StatData stat in EquipmentItems[type].ItemSo.EquipmentStats)
        {
            PlayerController.Instance.StatManager.ApplyStatEffect(stat.Type, StatValueType.Equipment, stat.Value);
        }

        data.IsEquipped = true;
        Debug.Log($"아이템 장착 : {data.ItemSo.ItemName}");
        OnEquipmentChanged?.Invoke(type);
    }

    public void UnEquipItem(EquipmentType type)
    {
        if (EquipmentItems.ContainsKey(type) && EquipmentItems[type] != null)
        {
            EquipmentItem item = EquipmentItems[type];
            InventoryManager.Instance.AddItem(item.ItemSo);
            foreach (StatData stat in item.ItemSo.EquipmentStats)
            {
                PlayerController.Instance.StatManager.ApplyStatEffect(stat.Type, StatValueType.Equipment, -stat.Value);
            }

            item.IsEquipped = false;
            EquipmentItems[type] = null;
            Debug.Log($"아이템 장착 해제 : {item.ItemSo.ItemName}");
            OnEquipmentChanged?.Invoke(type);
        }
    }
}