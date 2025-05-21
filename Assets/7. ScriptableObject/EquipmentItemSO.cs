using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EquipmentItemData", menuName = "Item/Equipment", order = 0)]
public class EquipmentItemSO : ItemSO
{
    public EquipmentType EquipmentType;
    public List<StatData> EquipmentStats;
}