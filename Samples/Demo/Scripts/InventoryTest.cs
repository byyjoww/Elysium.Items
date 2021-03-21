using Elysium.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    public InventorySO inventory;
    public IInventoryElementDatabase items;

    private void OnValidate()
    {
        items.Refresh();
    }

    [ContextMenu("Add Dummy Item")]
    private void AddItemToInventory()
    {
        bool added = inventory.Add(items.Elements[0] as IInventoryElement, 20);
        Debug.Log(added);
    }

    [ContextMenu("Contains")]
    private void CheckIfInventoryContains()
    {
        bool contains = inventory.Contains(items.Elements[0] as IInventoryElement);
        Debug.Log(contains);
    }

    [ContextMenu("Count")]
    private void CheckQuantity()
    {
        int qty = inventory.Quantity(items.Elements[0] as IInventoryElement);
        Debug.Log(qty);
    }

    [ContextMenu("Remove")]
    private void RemoveItemFromInventory()
    {
        bool removed = inventory.Remove(items.Elements[0] as IInventoryElement, 7);
        Debug.Log(removed);
    }
}
