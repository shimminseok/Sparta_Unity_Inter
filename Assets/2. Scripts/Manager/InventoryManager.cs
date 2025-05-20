using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : Singleton<InventoryManager>
{
    private List<ItemData> inventory = new List<ItemData>();


    public event Action<int> OnInventorySlotUpdate;

    private void RemoveItem(int id)
    {
        inventory.RemoveAll(x => x.Id == id);
    }

    public void AddItem(ItemData item)
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

    public void UseItem(ItemData item)
    {
        switch (item.ItemType)
        {
            case ItemType.Consume:
                foreach (ItemEffect itemEffect in item.Effects)
                {
                }

                break;
        }
    }

    public void EquipItem(ItemData item)
    {
        //장비 장착(인벤토리에서 삭제)
    }

    public void DropItem(int id, int amount)
    {
        ItemData data = inventory.Find(x => x.Id == id);
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
    private void AddStackableItem(ItemData item)
    {
        ItemData findItem = inventory.Find(x => x != null && x.Id == item.Id);
        int      index    = 0;
        if (findItem == null)
        {
            // To Do 인벤토리가 꽉찼는지 확인
            index = inventory.IndexOf(null);
            if (index < 0)
            {
                Debug.Log("인벤토리가 가득 찼습니다.");
                return;
            }

            findItem = item.DeepCopy();
            inventory[index] = findItem;
        }
        else
        {
            index = inventory.IndexOf(findItem);
            findItem.Quantity += item.Quantity;
        }

        OnInventorySlotUpdate?.Invoke(index);
    }

    /// <summary>
    /// 비스택형 아이템을 추가하는 함수
    /// </summary>
    /// <param name="item"></param>
    private void AddNonStackableItem(ItemData item)
    {
        int emptySlotCount = inventory.Count(x => x == null);

        if (emptySlotCount < item.Quantity)
        {
            Debug.Log("인벤토리 공간이 부족하여 구매할 수 없습니다.");
            return;
        }

        for (int i = 0; i < item.Quantity; i++)
        {
            int index = inventory.IndexOf(null);
            if (index >= 0)
            {
                ItemData data = item.DeepCopy();
                item.Quantity = 1;
                inventory[index] = item;
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