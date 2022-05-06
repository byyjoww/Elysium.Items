using Elysium.Core;
using Elysium.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items
{
    public class LimitedItemStackCollection : ItemStackCollection
    {
        private IItemStack[] stacks = default;

        public override IEnumerable<IItemStack> Stacks => stacks;
        public IEnumerable<IItemStack> EmptyStacks => Stacks.Where(x => x.IsEmpty);
        public IEnumerable<IItemStack> UsedStacks => Stacks.Where(x => !x.IsEmpty);
        public IntValueRange Capacity { get; private set; }

        public LimitedItemStackCollection(IEnumerable<IItemStack> _stacks, Capacity _capacity, int _currentCapacity)
        {
            if (_stacks.Count() > _currentCapacity)
            {
                throw new System.Exception($"number of item stacks ({_stacks.Count()}) is bigger than inventory capacity ({_currentCapacity})");
            }

            Capacity = new IntValueRange(_currentCapacity, _capacity.Min, _capacity.Max, _capacity.Default);
            Capacity.OnChanged += TriggerOnValueChanged;
            InitializeStacks(_stacks);
        }       

        public override bool Add(IItem _item, int _quantity)
        {
            if (!HasSpace(_item, _quantity)) { return false; }
            int remainingAfterAddingToExistingStacks = AddToExistingStacks(_item, _quantity);
            int remainingAfterAddingToNewStacks = AddToNewStacks(_item, remainingAfterAddingToExistingStacks);
            if (remainingAfterAddingToNewStacks > 0) { throw new System.Exception($"There are still {remainingAfterAddingToNewStacks} items that need to be added to the inventory"); }
            return true;
        }

        public override bool Remove(IItem _item, int _quantity)
        {
            int currentNumOfItems = Quantity(_item);
            if (currentNumOfItems < _quantity) { return false; }
            int remaining = RemoveFromExistingStacks(_item, _quantity);
            if (remaining > 0) { throw new System.Exception($"There are still {remaining} items that need to be removed from the inventory"); }
            return true;
        }

        public bool Expand(int _quantity)
        {
            bool success = Capacity.Add(_quantity);
            if (success)
            {
                Array.Resize(ref stacks, Capacity.Value);
                for (int i = 0; i < Stacks.Count(); i++)
                {
                    if (stacks[i] == null) 
                    { 
                        stacks[i] = CreateStack();
                    }
                }
            }
            return success;
        }

        public bool Shrink(int _quantity, out IEnumerable<IItemStack> _excessItems)
        {
            bool success = Capacity.Remove(_quantity);
            _excessItems = GetExcessStacks();
            if (success) { Array.Resize(ref stacks, Capacity.Value); }
            return success;
        }

        protected bool HasSpace(IItem _item, int _quantity)
        {
            if (_item is null) { return false; }
            IItemStack existing = stacks.FirstOrDefault(x => x.Item == _item);
            int existingStackSpace = existing != null ? _item.MaxStack - existing.Quantity : 0;
            int emptyStackSpace = EmptyStacks.Count() * _item.MaxStack;
            return (emptyStackSpace + existingStackSpace) >= _quantity;
        }

        protected int AddToStack(IItem _item, int _remaining, IItemStack _stack)
        {
            int remaining = _remaining;
            int spaceInStack = _item.MaxStack - _stack.Quantity;
            int qtyToAdd = Mathf.Min(remaining, spaceInStack);
            _stack.Add(qtyToAdd);
            remaining -= qtyToAdd;
            return remaining;
        }

        protected int AddToExistingStacks(IItem _item, int _remaining)
        {
            int remaining = _remaining;
            IEnumerable<IItemStack> existing = GetStacksContainingItem(_item);
            for (int i = 0; i < existing.Count(); i++)
            {
                if (remaining <= 0) { break; }
                IItemStack stack = existing.ElementAt(i);
                remaining = AddToStack(_item, remaining, stack);
            }

            return remaining;
        }

        protected virtual int AddToNewStacks(IItem _item, int _remaining)
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

        protected virtual int RemoveFromStack(int _remaining, IItemStack _stack)
        {
            int qtyToRemove = Mathf.Min(_remaining, _stack.Quantity);
            _stack.Remove(qtyToRemove);
            _remaining -= qtyToRemove;
            return _remaining;
        }

        protected int RemoveFromExistingStacks(IItem _item, int _quantity)
        {
            int remaining = _quantity;
            IEnumerable<IItemStack> existing = GetStacksContainingItem(_item).Reverse();
            int numOfExistingStacks = existing.Count();
            for (int i = numOfExistingStacks - 1; i >= 0; i--)
            {
                if (remaining <= 0) { break; }
                remaining = RemoveFromStack(remaining, existing.ElementAt(i));
            }

            return remaining;
        }

        private IEnumerable<IItemStack> GetExcessStacks()
        {
            int excess = stacks.Length - Capacity.Value;
            if (excess <= 0) { return new List<IItemStack>(); }
            var items = new List<IItemStack>();
            for (int i = 0; i < excess; i++)
            {
                IItemStack stack = stacks.Last();
                stack.OnValueChanged -= TriggerOnValueChanged;
                items.Add(ItemStack.WithContents(stack.Item, stack.Quantity));
            }
            return items;
        }

        private void InitializeStacks(IEnumerable<IItemStack> _stacks)
        {
            stacks = new IItemStack[Capacity.Value];
            for (int i = 0; i < Stacks.Count(); i++)
            {
                stacks[i] = i < _stacks.Count() 
                    ? CreateStack(_stacks.ElementAt(i)) 
                    : CreateStack();
            }
        }

        ~LimitedItemStackCollection()
        {
            Capacity.OnChanged -= TriggerOnValueChanged;
        }
    }
}
