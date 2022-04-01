using System;
using UnityEngine;

namespace Elysium.Items
{
    [System.Serializable]
    public class ItemStack : IItemStack
    {
        private const int MIN_VALUE = 0;
        private const int MAX_VALUE = int.MaxValue;

        private Guid id = default;
        private IItem item = default;
        private int quantity = default;

        public Guid ID => id;
        public IItem Item => item;
        public int Quantity => quantity;
        public bool IsFull => item != null && quantity >= Item.MaxStack;
        public bool IsEmpty => item == null;

        public ItemStack(Guid _stackID)
        {
            id = _stackID;
            Set(null, MIN_VALUE);
        }

        public static ItemStack New()
        {
            var stack = new ItemStack(Guid.NewGuid());
            stack.Set(null, MIN_VALUE);
            return stack;
        }

        public static ItemStack WithContents(IItem _item, int _amount)
        {
            var stack = new ItemStack(Guid.NewGuid());
            stack.Set(_item, _amount);
            return stack;
        }

        public void Add(int _quantity)
        {
            Set(quantity + _quantity);
        }

        public void Remove(int _quantity)
        {
            Set(quantity - _quantity);
        }

        public void Set(IItem _item, int _value)
        {
            Set(_item);
            Set(_value);
        }

        public void Set(IItem _item)
        {
            item = _item;
        }

        public void Set(int _value)
        {
            quantity = Mathf.Clamp(_value, MIN_VALUE, MAX_VALUE);
            if (quantity <= 0) { Empty(); }
        }

        public void Empty()
        {
            quantity = 0;
            item = null;
        }

        public new string ToString()
        {
            return $"x{quantity} {item.Name}";
        }
    }
}
