using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Elysium.Items
{
    public class UnlimitedItemStackCollection : ItemStackCollection
    {
        private List<IItemStack> stacks = default;

        public override IEnumerable<IItemStack> Stacks => stacks;

        public UnlimitedItemStackCollection(IEnumerable<IItemStack> _stacks)
        {
            stacks = _stacks.ToList();
            BindStacks(stacks);
        }

        protected override void ResetItemStacks()
        {
            DisposeStacks(stacks);
            stacks = new List<IItemStack>();
        }

        protected override bool HasSpace(IItem _item, int _quantity)
        {
            return true;
        }

        protected override int AddToNewStacks(IItem _item, int _remaining)
        {
            int numOfNewStacks = Mathf.CeilToInt((float)_remaining / (float)_item.MaxStack);
            for (int i = 0; i < numOfNewStacks; i++)
            {
                if (_remaining <= 0) { break; }
                IItemStack stack = CreateStack();
                _remaining = AddToStack(_item, _remaining, stack);
                stack.Set(_item);
                stacks.Add(stack);
            }

            return _remaining;
        }

        protected override int RemoveFromStack(int _remaining, IItemStack _stack)
        {
            int qtyToRemove = Mathf.Min(_remaining, _stack.Quantity);
            _stack.Remove(qtyToRemove);
            if (_stack.IsEmpty) 
            {
                DisposeStacks(_stack);
                stacks.Remove(_stack);
            }
            _remaining -= qtyToRemove;
            return _remaining;
        }

        protected override void BindStacks(IItemStack _stack)
        {
            base.BindStacks(_stack);
            void CullStack()
            {
                _stack.OnEmpty -= CullStack;
                DisposeStacks(_stack);
                stacks.Remove(_stack);
                TriggerOnValueChanged();
            }
            _stack.OnEmpty += CullStack;
        }
    }
}
