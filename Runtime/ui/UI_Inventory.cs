using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Elysium.Utils.Attributes;
using Elysium.Utils;

namespace Elysium.Items
{
    public class UI_Inventory : MonoBehaviour
    {
        [Header("Inventory")]
        [SerializeField] protected InventorySO inventory = default;
        [SerializeField] protected ItemStackEventSO useItemEvent = default;

        [Header("Settings")]
        [SerializeField] protected int minLastRowElements = 0;
        [SerializeField] protected bool minInventoryElementsByCapacity = true;
        [SerializeField, ConditionalField("minInventoryElementsByCapacity", true)] protected int minInventoryElements = 0;

        [Header("Prefabs")]
        [SerializeField] protected UI_InventorySlot pfInventoryElement = default;
        [SerializeField] protected Transform tInventoryElement = default;

        protected List<UI_InventorySlot> instantiatedSlots = new List<UI_InventorySlot>();        
        protected InventoryFilters.BaseFilter activeFilter = InventoryFilters.GetEmptyFilter();
        protected bool initialized = false;

        protected int MinInventoryElements
        {
            get
            {
                if (minInventoryElementsByCapacity) { return inventory.Capacity; }
                return minInventoryElements;
            }
        }

        public void ChangeActiveFilter(int _filter)
        {
            activeFilter = InventoryFilters.GetFilterAtPosition(_filter);
            RefreshUI();
        }

        protected void OnEnable()
        {
            if (!initialized) { Init(); }
            RefreshUI();
        }

        protected void OnDisable()
        {
            inventory.OnValueChanged -= RefreshUI;
            initialized = false;
        }

        protected void Init()
        {
            inventory.OnValueChanged += RefreshUI;
            initialized = true;
        }

        protected void RefreshUI()
        {
            ClearOldElements();

            foreach (var stack in inventory.ItemStacks)
            {
                if (!activeFilter.Evaluate(stack.InventoryElement)) { continue; }
                InstantiateInventoryElement(stack);
            }

            InstantiateEmptyElements();
        }

        protected void InstantiateInventoryElement(ItemStack _stack)
        {
            UI_InventorySlot slot = Instantiate(pfInventoryElement, tInventoryElement);
            slot.Setup(inventory, _stack, useItemEvent);
            instantiatedSlots.Add(slot);
        }

        protected void InstantiateEmptyElements()
        {
            while (instantiatedSlots.Count < MinInventoryElements)
            {
                InstantiateInventoryElement(ItemStack.Empty());
            }

            if (minLastRowElements == 0) { return; }

            while (instantiatedSlots.Count % minLastRowElements != 0)
            {
                InstantiateInventoryElement(ItemStack.Empty());
            }
        }

        protected void ClearOldElements()
        {
            foreach (UI_InventorySlot slot in instantiatedSlots)
            {
                Destroy(slot.gameObject);
            }

            instantiatedSlots.Clear();
        }
    }
}