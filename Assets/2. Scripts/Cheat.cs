using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    [SerializeField] List<ItemSO> items = new List<ItemSO>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            foreach (ItemSO item in items)
            {
                if (item.IsStackable)
                    InventoryManager.Instance.AddItem(item, 10);
                else
                    InventoryManager.Instance.AddItem(item, 1);
            }
        }
    }
}