using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items
{
    public abstract class ItemStackCollection : IItemStackCollection
    {
        public abstract IEnumerable<IItemStack> Stacks { get; }

        public event UnityAction OnItemsChanged = delegate { };
        public event UnityAction OnValueChanged = delegate { };

        public virtual bool Add(IItem _item, int _quantity)
        {
            if (!HasSpace(_item, _quantity)) { return false; }

            int remainingAfterAddingToExistingStacks = AddToExistingStacks(_item, _quantity);
            // Debug.Log($"Added {_quantity - remainingAfterAddingToExistingStacks} items to existing stacks.");

            int remainingAfterAddingToNewStacks = AddToNewStacks(_item, remainingAfterAddingToExistingStacks);
            // Debug.Log($"Added {remainingAfterAddingToExistingStacks - remainingAfterAddingToNewStacks} items to new stacks.");

            if (remainingAfterAddingToNewStacks > 0) { throw new System.Exception($"There are still {remainingAfterAddingToNewStacks} items that need to be added to the inventory"); }
            TriggerOnValueChanged();
            return true;
        }

        public virtual bool Remove(IItem _item, int _quantity)
        {
            int currentNumOfItems = Quantity(_item);
            if (currentNumOfItems < _quantity) { return false; }

            int remaining = RemoveFromExistingStacks(_item, _quantity);

            if (remaining > 0) { throw new System.Exception($"There are still {remaining} items that need to be removed from the inventory"); }
            TriggerOnValueChanged();
            return true;
        }

        public bool Contains(IItem _item)
        {
            return Stacks.Any(x => !x.IsEmpty && x.Item == _item);
        }

        public virtual int Quantity(IItem _item)
        {
            return Stacks.Where(x => x.Item == _item).Sum(x => x.Quantity);
        }

        public virtual void Empty()
        {
            ResetItemStacks();
            TriggerOnValueChanged();
        }

        protected abstract void ResetItemStacks();

        protected abstract bool HasSpace(IItem _item, int _quantity);

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

        protected abstract int AddToNewStacks(IItem _item, int _remaining);

        protected int AddToStack(IItem _item, int _remaining, IItemStack _stack)
        {
            int remaining = _remaining;
            int spaceInStack = _item.MaxStack - _stack.Quantity;
            int qtyToAdd = Mathf.Min(remaining, spaceInStack);
            _stack.Add(qtyToAdd);
            remaining -= qtyToAdd;
            return remaining;
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

        protected abstract int RemoveFromStack(int _remaining, IItemStack _stack);

        protected IEnumerable<IItemStack> GetStacksContainingItem(IItem _item)
        {
            return Stacks.Where(x => x.Item == _item);
        }

        protected void TriggerOnItemsChanged()
        {
            OnItemsChanged?.Invoke();
        }

        protected void TriggerOnValueChanged()
        {
            OnValueChanged?.Invoke();
            TriggerOnItemsChanged();
        }
    }
}
