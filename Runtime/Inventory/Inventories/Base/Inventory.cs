using Elysium.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.Events;

namespace Elysium.Items
{
    public abstract class Inventory : IInventory
    {
        public abstract IItemStackCollection Items { get; }
        public int NumOfSlots => Items.Stacks.Count();

        public event UnityAction OnValueChanged = delegate { };

        protected Inventory() { }

        public bool Add(IItem _item, int _quantity)
        {
            return Items.Add(_item, _quantity);
        }

        public bool Remove(IItem _item, int _quantity)
        {
            return Items.Remove(_item, _quantity);
        }

        public bool Contains(IItem _item)
        {
            return Items.Contains(_item);
        }

        public int Quantity(IItem _item)
        {
            return Items.Quantity(_item);
        }

        public void Empty()
        {
            Items.Empty();
        }

        protected void TriggerOnValueChanged()
        {
            OnValueChanged?.Invoke();
        }

        public IEnumerator<IItemStack> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        ~Inventory()
        {
            Items.OnValueChanged.RemoveAllListeners();
        }
    }
}
