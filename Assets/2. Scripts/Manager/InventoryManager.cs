using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public ItemSO ItemSo;
    public int Quantity;
}

public class InventoryManager : Singleton<InventoryManager>
{
    private List<InventoryItem> inventory = new List<InventoryItem>();


    public event Action<int> OnInventorySlotUpdate;

    private void RemoveItem(int id)
    {
        inventory.RemoveAll(x => x.ItemSo.Id == id);
    }

    public void AddItem(ItemSO item)
    {
        if (item.IsStackable)
        {
            AddStackableItem(item);
        }
        else
        {
            AddNonStackableItem(item);
        }
    }

    public void UseItem(int id, int amount)
    {
        InventoryItem inventoryItem = inventory.Find(x => x.ItemSo.Id == id);
        if (inventoryItem.Quantity < amount)
            return;
        ItemSO itemSo = inventoryItem.ItemSo;
        switch (inventoryItem.ItemSo.ItemType)
        {
            case ItemType.Consume:
                foreach (StatusEffectData itemSoStatusEffect in itemSo.StatusEffects)
                {
                    PlayerController.Instance.StatusEffectManager.ApplyEffect(BuffFactory.CreateBuff(itemSoStatusEffect));
                }

                inventoryItem.Quantity -= amount;
                break;
        }

        if (inventoryItem.Quantity <= 0)
            RemoveItem(id);
    }

    public void EquipItem(ItemSO item)
    {
        //장비 장착(인벤토리에서 삭제)
    }

    public void DropItem(int id, int amount)
    {
        InventoryItem data = inventory.Find(x => x.ItemSo.Id == id);
        if (data == null || data.Quantity < amount)
            return;

        data.Quantity -= amount;
        if (data.Quantity == 0)
            RemoveItem(id);
    }

    /// <summary>
    /// 스택형 아이템을 추가하는 함수
    /// </summary>
    /// <param name="item"></param>
    private void AddStackableItem(ItemSO itemSo, int amount = 1)
    {
        InventoryItem findItem = inventory.Find(x => x != null && x.ItemSo == itemSo);
        int           index    = 0;
        if (findItem == null)
        {
            // To Do 인벤토리가 꽉찼는지 확인
            index = inventory.IndexOf(null);
            if (index < 0)
            {
                Debug.Log("인벤토리가 가득 찼습니다.");
                return;
            }

            findItem = new InventoryItem() { ItemSo = itemSo, Quantity = 1 };
            inventory[index] = findItem;
        }
        else
        {
            index = inventory.IndexOf(findItem);
            findItem.Quantity += amount;
        }

        OnInventorySlotUpdate?.Invoke(index);
    }

    /// <summary>
    /// 비스택형 아이템을 추가하는 함수
    /// </summary>
    /// <param name="item"></param>
    private void AddNonStackableItem(ItemSO item, int amount = 1)
    {
        int emptySlotCount = inventory.Count(x => x == null);

        if (emptySlotCount < amount)
        {
            Debug.Log("인벤토리 공간이 부족하여 구매할 수 없습니다.");
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            int index = inventory.IndexOf(null);
            if (index >= 0)
            {
                InventoryItem data = new InventoryItem() { ItemSo = item, Quantity = 1 };
                inventory[index] = data;
                OnInventorySlotUpdate?.Invoke(index);
            }
        }
    }

    public void SwichItem(int from, int to)
    {
        (inventory[from], inventory[to]) = (inventory[to], inventory[from]);


        OnInventorySlotUpdate?.Invoke(from);
        OnInventorySlotUpdate?.Invoke(to);
    }
}