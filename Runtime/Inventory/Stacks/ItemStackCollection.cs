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

        public abstract bool Add(IItem _item, int _quantity);

        public abstract bool Remove(IItem _item, int _quantity);

        public virtual bool Contains(IItem _item)
        {
            return Stacks.Any(x => !x.IsEmpty && x.Item.Equals(_item));
        }

        public virtual void Empty()
        {
            foreach (var stack in Stacks)
            {
                stack.Empty();
            }
        }

        public virtual int Quantity(IItem _item)
        {
            return Stacks.Where(x => !x.IsEmpty && x.Item.Equals(_item)).Sum(x => x.Quantity);
        }

        protected virtual IEnumerable<IItemStack> GetStacksContainingItem(IItem _item)
        {
            return Stacks.Where(x => x.Item == _item);
        }

        protected virtual IItemStack CreateStack()
        {
            var stack = ItemStack.New();
            stack.OnValueChanged += TriggerOnValueChanged;
            return stack;
        }

        protected virtual IItemStack CreateStack(IItem _item, int _quantity)
        {
            var stack = ItemStack.WithContents(_item, _quantity);
            stack.OnValueChanged += TriggerOnValueChanged;
            return stack;
        }

        protected virtual IItemStack CreateStack(IItemStack _stack)
        {
            var stack = ItemStack.WithContents(_stack.Item, _stack.Quantity);
            stack.OnValueChanged += TriggerOnValueChanged;
            return stack;
        }

        protected void TriggerOnValueChanged()
        {
            OnValueChanged?.Invoke();
        }

        public IEnumerator<IItemStack> GetEnumerator()
        {
            return Stacks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        ~ItemStackCollection()
        {
            foreach (var stack in Stacks)
            {
                stack.OnValueChanged -= TriggerOnValueChanged;
            }
        }
    }
}
