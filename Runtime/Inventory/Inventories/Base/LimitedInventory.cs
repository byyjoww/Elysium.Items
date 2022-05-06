using Elysium.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    public class LimitedInventory : Inventory, IExpandable
    {
        private LimitedItemStackCollection items = default;

        public override IItemStackCollection Items => items;
        public int AvailableExpansion => items.Capacity.Max - items.Capacity.Value;
        public int AvailableShrink => items.Capacity.Value - items.Capacity.Min;

        private LimitedInventory(IEnumerable<IItemStack> _stacks, Capacity _capacity, int _currentCapacity)
        {
            items = new LimitedItemStackCollection(_stacks, _capacity, _currentCapacity);
            items.OnValueChanged.AddListener(TriggerOnValueChanged);
        }

        public static LimitedInventory New(Capacity _capacity)
        {
            return new LimitedInventory(new List<IItemStack>(), _capacity, _capacity.Default);
        }

        public static LimitedInventory New(Capacity _capacity, int _currentCapacity)
        {
            return new LimitedInventory(new List<IItemStack>(), _capacity, _currentCapacity);
        }

        public static LimitedInventory New(Capacity _capacity, IEnumerable<IItemStack> _stacks)
        {
            return new LimitedInventory(_stacks, _capacity, _capacity.Default);
        }

        public static LimitedInventory New(Capacity _capacity, int _currentCapacity, IEnumerable<IItemStack> _stacks)
        {
            return new LimitedInventory(_stacks, _capacity, _currentCapacity);
        }

        public bool Expand(int _quantity)
        {            
            return items.Expand(_quantity);
        }

        public bool Shrink(int _quantity, out IEnumerable<IItemStack> _excessItems)
        {
            return items.Shrink(_quantity, out _excessItems);
        }
    }
}
