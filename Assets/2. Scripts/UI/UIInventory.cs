using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : UIBase<UIInventory>, IUIBase
{
    [SerializeField] private InventorySlot[] inventorySlots;


    public InventorySlot SelectedItem { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitializeSlots();
        InventoryManager.Instance.OnInventorySlotUpdate += UpdateInventorySlot;
    }

    private void InitializeSlots()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].SetItem(i, InventoryManager.Instance.Inventory[i]);
        }
    }

    public void SelecteItem(InventorySlot item)
    {
        if (SelectedItem != null && SelectedItem != item)
            SelectedItem.DeSelectedSlot();

        SelectedItem = item;
    }

    void UpdateInventorySlot(int index)
    {
        if (index < 0 || index >= inventorySlots.Length)
            return;

        InventoryItem itemData = InventoryManager.Instance.Inventory[index];

        inventorySlots[index].SetItem(index, itemData);
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }
}