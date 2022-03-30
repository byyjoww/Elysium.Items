using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Elysium.Items
{
    public class UnlimitedItemStackCollection : ItemStackCollection
    {
        private List<ItemStack> stacks = default;

        public override IEnumerable<IItemStack> Stacks => stacks;

        public UnlimitedItemStackCollection()
        {
            ResetItemStacks();
        }

        protected override void ResetItemStacks()
        {
            stacks = new List<ItemStack>();
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
                ItemStack stack = new ItemStack();
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
            if (_stack.IsEmpty && _stack is ItemStack) { stacks.Remove(_stack as ItemStack); }
            _remaining -= qtyToRemove;
            return _remaining;
        }
    }
}
