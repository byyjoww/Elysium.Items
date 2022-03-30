using System;
using UnityEngine;

namespace Elysium.Items
{
    [System.Serializable]
    public class ItemStack : IItemStack
    {
        private const int MIN_VALUE = 0;
        private const int MAX_VALUE = int.MaxValue;

        private string id = default;
        private IItem item = default;
        private int quantity = default;

        public string ID => id;
        public IItem Item => item;
        public int Quantity => quantity;
        public bool IsFull => item != null && quantity >= Item.MaxStack;
        public bool IsEmpty => item == null;

        public ItemStack()
        {
            id = Guid.NewGuid().ToString();
            Set(null, MIN_VALUE);
        }

        public ItemStack(IItem _item, int _quantity)
        {
            id = Guid.NewGuid().ToString();
            Set(_item, _quantity);
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
    }
}
