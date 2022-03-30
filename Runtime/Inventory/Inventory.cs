using Elysium.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items
{
    public abstract class Inventory : IInventory
    {
        protected IItemStackCollection items = default;

        public IItemStackCollection Items => items;
        public int NumOfSlots => Items.Stacks.Count();

        public event UnityAction OnItemsChanged = delegate { };
        public event UnityAction OnValueChanged = delegate { };

        public Inventory()
        {
            
        }

        public bool Add(IItem _item, int _quantity)
        {
            return items.Add(_item, _quantity);
        }

        public bool Remove(IItem _item, int _quantity)
        {
            return items.Remove(_item, _quantity);
        }

        public bool Contains(IItem _item)
        {
            return items.Contains(_item);
        }

        public int Quantity(IItem _item)
        {
            return items.Quantity(_item);
        }

        public void Empty()
        {
            items.Empty();
        }

        public void Swap(IItemStack _origin, IItemStack _destination)
        {
            IItem item = _destination.Item;
            int quantity = _destination.Quantity;

            _destination.Set(_origin.Item, _origin.Quantity);
            _origin.Set(item, quantity);
            OnValueChanged?.Invoke();
        }

        protected void TriggerOnItemsChanged()
        {
            OnItemsChanged?.Invoke();
        }

        protected void TriggerOnValueChanged()
        {
            OnValueChanged?.Invoke();
        }        

        ~Inventory()
        {
            items.OnItemsChanged -= TriggerOnItemsChanged;
            items.OnValueChanged -= TriggerOnValueChanged;
        }
    }
}
