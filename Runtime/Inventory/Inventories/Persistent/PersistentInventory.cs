using Elysium.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Elysium.Items
{
    public abstract class PersistentInventory : IPersistentInventory
    {
        protected Guid inventoryID = default;
        protected abstract IInventory Inventory { get; }
        public IItemStackCollection Items
        {
            get
            {
                if (Inventory == null) { return new NullItemStackCollection(); }
                return Inventory.Items;
            }
        }

        public event UnityAction OnValueChanged = delegate { };
        public event UnityAction<IPersistent> OnPersistentDataChanged = delegate { };

        protected PersistentInventory(Guid _inventoryID)
        {
            this.inventoryID = _inventoryID;            
        }

        public bool Add(IItem _item, int _quantity)
        {
            return Inventory.Add(_item, _quantity);
        }

        public bool Remove(IItem _item, int _quantity)
        {
            return Inventory.Remove(_item, _quantity);
        }

        public int Quantity(IItem _item)
        {
            return Inventory.Quantity(_item);
        }

        public bool Contains(IItem _item)
        {
            return Inventory.Contains(_item);
        }

        public void Empty()
        {
            Inventory.Empty();
        }

        public IEnumerator<IItemStack> GetEnumerator()
        {
            return Inventory.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract void Load(ILoader _loader);

        public abstract void Save(ISaver _saver);

        protected void TriggerOnValueChanged()
        {
            OnValueChanged?.Invoke();
            OnPersistentDataChanged?.Invoke(this);
        }

        protected virtual void OnDestroy()
        {
            Inventory.OnValueChanged -= TriggerOnValueChanged;
        }
    }
}
