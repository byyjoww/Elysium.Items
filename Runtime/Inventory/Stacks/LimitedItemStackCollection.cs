using Elysium.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.Items
{
    public class LimitedItemStackCollection : ItemStackCollection
    {
        private Capacity config = default;
        private ItemStack[] stacks = default;

        public override IEnumerable<IItemStack> Stacks => stacks;
        public IEnumerable<IItemStack> EmptyStacks => Stacks.Where(x => x.IsEmpty);
        public IEnumerable<IItemStack> UsedStacks => Stacks.Where(x => !x.IsEmpty);
        public IntValueRange Capacity { get; private set; }

        public LimitedItemStackCollection(Capacity _config)
        {
            config = _config;
            Capacity = new IntValueRange(config.Default, config.Min, config.Max, config.Default);
            Capacity.OnChanged += TriggerOnValueChanged;
            ResetItemStacks();
        }

        public bool Expand(int _quantity)
        {
            bool success = Capacity.Add(_quantity);
            if (success) 
            { 
                Array.Resize(ref stacks, Capacity.Value);
                for (int i = 0; i < stacks.Length; i++)
                {
                    if (stacks[i] == null) { stacks[i] = new ItemStack(); }
                }
            }
            return success;
        }

        public bool Shrink(int _quantity, out IEnumerable<IItemStack> _excessItems)
        {
            bool success = Capacity.Remove(_quantity);
            _excessItems = GetExcessItems();
            if (success) { Array.Resize(ref stacks, Capacity.Value); }
            return success;
        }        

        protected override void ResetItemStacks()
        {
            stacks = new ItemStack[Capacity.Value];
            for (int i = 0; i < stacks.Length; i++)
            {
                stacks[i] = new ItemStack();
            }
        }

        protected override bool HasSpace(IItem _item, int _quantity)
        {
            if (_item is null) { return false; }
            ItemStack existing = stacks.FirstOrDefault(x => x.Item == _item);
            int existingStackSpace = existing != null ? _item.MaxStack - existing.Quantity : 0;
            int emptyStackSpace = EmptyStacks.Count() * _item.MaxStack;
            return (emptyStackSpace + existingStackSpace) >= _quantity;
        }

        protected override int AddToNewStacks(IItem _item, int _remaining)
        {
            int remaining = _remaining;
            IEnumerable<IItemStack> empty = EmptyStacks.ToList();
            int numOfEmptyStacks = empty.Count();
            for (int i = 0; i < numOfEmptyStacks; i++)
            {
                if (remaining <= 0) { break; }
                IItemStack stack = empty.ElementAt(i);
                stack.Set(_item);
                remaining = AddToStack(_item, remaining, stack);
            }

            return remaining;
        }

        protected override int RemoveFromStack(int _remaining, IItemStack _stack)
        {
            int qtyToRemove = Mathf.Min(_remaining, _stack.Quantity);
            _stack.Remove(qtyToRemove);
            _remaining -= qtyToRemove;
            return _remaining;
        }

        private IEnumerable<IItemStack> GetExcessItems()
        {
            int excess = stacks.Length - Capacity.Value;
            if (excess <= 0) { return new List<IItemStack>(); }
            var items = new List<IItemStack>();
            for (int i = 0; i < excess; i++)
            {                
                IItemStack stack = stacks.Last();
                items.Add(new ItemStack(stack.Item, stack.Quantity));
            }            
            return items;
        }
    }
}
