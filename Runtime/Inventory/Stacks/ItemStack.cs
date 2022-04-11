using System;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items
{
    [System.Serializable]
    public class ItemStack : IItemStack
    {
        private const int MIN_VALUE = 0;
        private const int MAX_VALUE = int.MaxValue;

        private Guid id = default;
        private IItem item = null;
        private int quantity = MIN_VALUE;

        public Guid ID => id;
        public IItem Item => !IsEmpty ? item : new NullItem();
        public int Quantity => quantity;
        public bool IsFull => item != null && quantity >= Item.MaxStack;
        public bool IsEmpty => item is null;

        public event UnityAction OnValueChanged = delegate { };
        public event UnityAction OnFull = delegate { };
        public event UnityAction OnEmpty = delegate { };

        public ItemStack(Guid _stackID)
        {
            id = _stackID;
        }

        public static ItemStack New()
        {
            return new ItemStack(Guid.NewGuid());
        }

        public static ItemStack WithContents(IItem _item, int _amount)
        {
            var stack = new ItemStack(Guid.NewGuid());
            stack.item = _item;
            stack.quantity = Mathf.Clamp(_amount, MIN_VALUE, MAX_VALUE);
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
            var prev = (item, quantity);
            SetInternal(_item);
            SetInternal(_value);
            HandleEvents(prev);
        }

        public void Set(IItem _item)
        {
            var prev = (item, quantity);
            SetInternal(_item);
            HandleEvents(prev);
        }        

        public void Set(int _value)
        {
            var prev = (item, quantity);
            SetInternal(_value);
            HandleEvents(prev);
        }

        public void Empty()
        {
            var prev = (item, quantity);
            SetInternal(null);
            SetInternal(MIN_VALUE);
            HandleEvents(prev);
        }

        private void SetInternal(IItem _item)
        {
            item = _item;
        }

        private void SetInternal(int _value)
        {
            quantity = Mathf.Clamp(_value, MIN_VALUE, MAX_VALUE);
            if (quantity <= 0) 
            {
                item = null;
                quantity = 0;
            }            
        }        

        public bool Use(IItemUser _user, int _numOfItemsToUse = 1)
        {
            if (quantity < _numOfItemsToUse) { return false; }
            for (int i = 0; i < _numOfItemsToUse; i++)
            {
                item.Use(this, _user);
            }
            return true;
        }

        public void SwapContents(IItemStack _target)
        {
            IItem otherItem = _target.Item;
            int otherQuantity = _target.Quantity;

            _target.Set(this.item, this.quantity);
            Set(otherItem, otherQuantity);

            Debug.Log($"swapped stacks for {ToString()} and {_target.ToString()}");
        }

        public new string ToString()
        {
            return !IsEmpty ? $"x{quantity} {item.Name}" : "{empty}";
        }       
        
        private void HandleEvents((IItem, int) _prev)
        {
            if (_prev.Item1 != item || _prev.Item2 != quantity) { OnValueChanged?.Invoke(); }
            if (IsFull) { OnFull?.Invoke(); }
            if (IsEmpty) { OnEmpty?.Invoke(); }
        }
    }
}
