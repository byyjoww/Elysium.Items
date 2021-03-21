using Elysium.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUserTest : MonoBehaviour, IInventoryUser
{
    [SerializeField] private InventorySO inventory = default;
    [SerializeField] private ItemStackEventSO useItemEvent = default;

    [Header("Items")]
    [SerializeField] private IInventoryElementDatabase itemDatabase = default;

    public GameObject Object => gameObject;
    public InventorySO Inventory => inventory;

    public void Heal(int _amount)
    {
        Debug.Log($"Healing for {_amount}");
    }

    private void OnEnable()
    {
        useItemEvent.OnRaise += ActivateItem;
    }

    private void OnDisable()
    {
        useItemEvent.OnRaise -= ActivateItem;
    }

    private void ActivateItem(ItemStack _stack)
    {
        _stack.InventoryElement.Activate(_stack, this);
    }    

    [ContextMenu("Get Item Assortment")]
    private void GetPotion()
    {
        foreach (IInventoryElement item in itemDatabase.ElementsAsInterface)
        {
            inventory.Add(item, 1);
        }
    }

    private void OnValidate()
    {
        itemDatabase.Refresh();
    }
}
