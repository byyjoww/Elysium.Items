using System.Collections;
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

        public UnityEvent OnValueChanged { get; } = new UnityEvent();

        public virtual bool Add(IItem _item, int _quantity)
        {
            if (!HasSpace(_item, _quantity)) { return false; }

            int remainingAfterAddingToExistingStacks = AddToExistingStacks(_item, _quantity);
            // Debug.Log($"Added {_quantity - remainingAfterAddingToExistingStacks} items to existing stacks.");

            int remainingAfterAddingToNewStacks = AddToNewStacks(_item, remainingAfterAddingToExistingStacks);
            // Debug.Log($"Added {remainingAfterAddingToExistingStacks - remainingAfterAddingToNewStacks} items to new stacks.");

            if (remainingAfterAddingToNewStacks > 0) { throw new System.Exception($"There are still {remainingAfterAddingToNewStacks} items that need to be added to the inventory"); }
            return true;
        }

        public virtual bool Remove(IItem _item, int _quantity)
        {
            int currentNumOfItems = Quantity(_item);
            if (currentNumOfItems < _quantity) { return false; }

            int remaining = RemoveFromExistingStacks(_item, _quantity);

            if (remaining > 0) { throw new System.Exception($"There are still {remaining} items that need to be removed from the inventory"); }
            return true;
        }

        public bool Contains(IItem _item)
        {
            return Stacks.Any(x => !x.IsEmpty && x.Item.Equals(_item));
        }

        public virtual int Quantity(IItem _item)
        {
            return Stacks.Where(x => !x.IsEmpty && x.Item.Equals(_item)).Sum(x => x.Quantity);
        }

        public virtual void Empty()
        {
            ResetItemStacks();
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

        public IEnumerator<IItemStack> GetEnumerator()
        {
            return Stacks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected virtual IItemStack CreateStack()
        {
            var stack = ItemStack.New();
            BindStacks(stack);
            return stack;
        }

        protected virtual void BindStacks(IItemStack _stack)
        {
            _stack.OnValueChanged += TriggerOnValueChanged;
        }

        protected virtual void BindStacks(IEnumerable<IItemStack> _stacks)
        {
            foreach (var s in _stacks)
            {
                BindStacks(s);
            }
        }

        protected virtual void DisposeStacks(IItemStack _stack)
        {
            _stack.OnValueChanged -= TriggerOnValueChanged;
        }

        protected virtual void DisposeStacks(IEnumerable<IItemStack> _stacks)
        {
            foreach (var s in _stacks)
            {
                DisposeStacks(s);
            }
        }

        protected void TriggerOnValueChanged()
        {
            OnValueChanged?.Invoke();
        }

        ~ItemStackCollection()
        {
            DisposeStacks(Stacks);
        }
    }
}
