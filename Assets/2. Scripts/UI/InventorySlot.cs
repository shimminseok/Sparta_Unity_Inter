using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemQuantity;

    public InventoryItem InventoryItem { get; private set; }
    private bool isSelected;

    public bool IsEmpty => InventoryItem == null;
    public int  Index   { get; private set; }

    public void SetItem(int index, InventoryItem item)
    {
        Index = index;
        if (item == null)
        {
            EmptySlot();
            return;
        }

        InventoryItem = item;
        icon.sprite = item.ItemSo.ItemSprite;
        itemQuantity.text = $"x{item.Quantity}";
    }

    public void EmptySlot()
    {
        icon.sprite = null;
        InventoryItem = null;
        itemQuantity.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsEmpty || !EventSystem.current.IsPointerOverGameObject())
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isSelected)
                DeSelectedSlot();
            else
                SelectedSlot();

            isSelected = !isSelected;
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseOrEquipItem();
        }
    }

    public void OnClickSlot()
    {
        SelectedSlot();
    }

    public void SelectedSlot()
    {
        // UIInventory.Instance.SelecteItem(this);
    }

    public void DeSelectedSlot()
    {
    }

    public void UseOrEquipItem()
    {
        ItemSO itemSo = InventoryItem.ItemSo;
        switch (itemSo.ItemType)
        {
            case ItemType.Consume:
                InventoryManager.Instance.UseItem(Index, 1);
                break;
            case ItemType.Equipment:
                InventoryManager.Instance.EquipItem(Index);
                break;
        }
    }

    void SwichInvenSlot(InventorySlot swich)
    {
        InventoryManager.Instance.SwichItem(swich.Index, Index);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!DragManager.Instance.IsDragging)
            return;

        if (DragManager.Instance.DraggedInventoryItem != null)
        {
            SwichInvenSlot(DragManager.Instance.DraggedInventoryItem);
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            DragManager.Instance.StartDrag(this, UIInventory.Instance.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragManager.Instance.UpdateDrag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragManager.Instance.EndDrag();
    }
}