using Elysium.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items
{
    public abstract class InventoryComponent : MonoBehaviour, IPersistentInventory
    {
        [SerializeField] protected Guid inventoryID = default;
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

        protected virtual void Awake()
        {
            Inventory.OnValueChanged += TriggerOnValueChanged;
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

        protected void TriggerOnValueChanged()
        {
            OnValueChanged?.Invoke();
        }

        protected virtual void OnDestroy()
        {
            Inventory.OnValueChanged -= TriggerOnValueChanged;
        }

        public abstract void Load(ILoader _loader);

        public abstract void Save(ISaver _saver);
    }
}
