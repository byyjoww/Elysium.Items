using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items
{
    public class UnlimitedItemStackCollection : ItemStackCollection
    {
        private List<IItemStack> stacks = default;

        public override IEnumerable<IItemStack> Stacks => stacks;

        public UnlimitedItemStackCollection(IEnumerable<IItemStack> _stacks)
        {
            stacks = _stacks.ToList();
            stacks.ForEach(x => x.OnValueChanged += TriggerOnValueChanged);
        }

        public override bool Add(IItem _item, int _quantity)
        {
            int remainingAfterAddingToExistingStacks = AddToExistingStacks(_item, _quantity);
            // Debug.Log($"Added {_quantity - remainingAfterAddingToExistingStacks} items to existing stacks.");

            int remainingAfterAddingToNewStacks = AddToNewStacks(_item, remainingAfterAddingToExistingStacks);
            // Debug.Log($"Added {remainingAfterAddingToExistingStacks - remainingAfterAddingToNewStacks} items to new stacks.");

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

        public override void Empty()
        {            
            foreach (var stack in Stacks)
            {
                stack.OnValueChanged -= TriggerOnValueChanged;
            }
            stacks = new List<IItemStack>();
            TriggerOnValueChanged();
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
            int numOfNewStacks = Mathf.CeilToInt((float)_remaining / (float)_item.MaxStack);
            for (int i = 0; i < numOfNewStacks; i++)
            {
                if (_remaining <= 0) { break; }
                IItemStack stack = CreateStackWithContents(_item, 0);
                _remaining = AddToStack(_item, _remaining, stack);
                stacks.Add(stack);
            }

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

        protected virtual int RemoveFromStack(int _remaining, IItemStack _stack)
        {
            int qtyToRemove = Mathf.Min(_remaining, _stack.Quantity);
            _stack.Remove(qtyToRemove);
            if (_stack.IsEmpty) 
            {
                _stack.OnValueChanged -= TriggerOnValueChanged;
                stacks.Remove(_stack);
            }
            _remaining -= qtyToRemove;
            return _remaining;
        }

        protected virtual IItemStack CreateStackWithContents(IItem _item, int _quantity)
        {
            var stack = ItemStack.WithContents(_item, _quantity);
            BindStacks(stack);
            return stack;
        }

        protected virtual void BindStacks(IItemStack _stack)
        {
            _stack.OnValueChanged += TriggerOnValueChanged;
            void CullStack()
            {
                _stack.OnEmpty -= CullStack;
                _stack.OnValueChanged -= TriggerOnValueChanged;
                stacks.Remove(_stack);
                TriggerOnValueChanged();
            }
            _stack.OnEmpty += CullStack;
        }
    }
}
