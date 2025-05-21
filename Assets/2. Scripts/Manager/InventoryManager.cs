using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public ItemSO ItemSo;
    public int Quantity;

    public InventoryItem(ItemSO itemSo, int quantity)
    {
        ItemSo = itemSo;
        Quantity = quantity;
    }
}

public class InventoryManager : Singleton<InventoryManager>
{
    public List<InventoryItem> Inventory { get; private set; } = new List<InventoryItem>(
        Enumerable.Repeat<InventoryItem>(null, 14));


    public event Action<int> OnInventorySlotUpdate;

    private void RemoveItem(int index)
    {   
        Inventory[index] = null;
        OnInventorySlotUpdate?.Invoke(index);
    }

    public void AddItem(ItemSO item, int amount = 1)
    {
        if (item is EquipmentItemSO equipmentItemSo)
        {
            AddEquipmentItems(equipmentItemSo, amount);
        }
        else
        {
            AddStackableItem(item, amount);
        }
    }

    /// <summary>
    /// 스택형 아이템을 추가하는 함수
    /// </summary>
    /// <param name="itemSo"></param>
    /// <param name="amount"></param>
    private void AddStackableItem(ItemSO itemSo, int amount = 1)
    {
        InventoryItem findItem = Inventory.Find(x => x != null && x.ItemSo == itemSo);
        int           index    = 0;
        if (findItem == null)
        {
            // To Do 인벤토리가 꽉찼는지 확인
            index = Inventory.IndexOf(null);
            if (index < 0)
            {
                Debug.Log("인벤토리가 가득 찼습니다.");
                return;
            }

            if (itemSo.ItemType == ItemType.Equipment)
            {
                findItem = new EquipmentItem(itemSo as EquipmentItemSO);
            }
            else
            {
                findItem = new InventoryItem(itemSo, amount);
            }

            Inventory[index] = findItem;
        }
        else
        {
            index = Inventory.IndexOf(findItem);
            findItem.Quantity += amount;
        }

        OnInventorySlotUpdate?.Invoke(index);
    }

    /// <summary>
    /// 비스택형 아이템을 추가하는 함수
    /// </summary>
    /// <param name="itemSo"></param>
    /// <param name="amount"></param>
    private void AddNonStackableItem(ItemSO itemSo, int amount = 1)
    {
        int emptySlotCount = Inventory.Count(x => x == null);

        if (emptySlotCount < amount)
        {
            Debug.Log("인벤토리 공간이 부족하여 구매할 수 없습니다.");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            int index = Inventory.IndexOf(null);
            if (index >= 0)
            {
                InventoryItem data = new InventoryItem(itemSo, 1);
                Inventory[index] = data;
                OnInventorySlotUpdate?.Invoke(index);
            }
        }
    }

    private void AddEquipmentItems(EquipmentItemSO itemSo, int amount)
    {
        int emptySlots = Inventory.Count(x => x == null);
        if (emptySlots < amount)
        {
            Debug.Log("인벤토리 공간이 부족합니다.");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            int index = Inventory.IndexOf(null);
            if (index < 0) break;

            Inventory[index] = new EquipmentItem(itemSo);
            OnInventorySlotUpdate?.Invoke(index);
        }
    }

    public void UseItem(int index, int amount)
    {
        InventoryItem inventoryItem = Inventory[index];

        if (inventoryItem.Quantity < amount || inventoryItem.ItemSo == null
                                            || inventoryItem.ItemSo is not ConsumeItemSO consumeItem)
        {
            return;
        }

        foreach (StatusEffectData itemSoStatusEffect in consumeItem.StatusEffects)
        {
            PlayerController.Instance.StatusEffectManager.ApplyEffect(BuffFactory.CreateBuff(itemSoStatusEffect));
        }

        inventoryItem.Quantity -= amount;

        if (inventoryItem.Quantity <= 0)
            RemoveItem(index);

        OnInventorySlotUpdate?.Invoke(index);
    }

    public void EquipItem(int index)
    {
        InventoryItem inventoryItem = Inventory[index];
        EquipmentManager.Instance.EquipItem(inventoryItem as EquipmentItem);
        inventoryItem.Quantity -= 1;
        if (inventoryItem.Quantity <= 0)
            RemoveItem(index);
    }

    public void DropItem(int index, int amount)
    {
        InventoryItem data = Inventory[index];
        if (data == null || data.Quantity < amount)
            return;

        data.Quantity -= amount;
        if (data.Quantity == 0)
            RemoveItem(index);
    }


    public void SwichItem(int from, int to)
    {
        (Inventory[from], Inventory[to]) = (Inventory[to], Inventory[from]);


        OnInventorySlotUpdate?.Invoke(from);
        OnInventorySlotUpdate?.Invoke(to);
    }
}