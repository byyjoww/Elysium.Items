using Elysium.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items
{
    public abstract class InventorySO : ScriptableObject, IPersistentInventory
    {
        [SerializeField] protected Guid inventoryID = Guid.NewGuid();

        protected abstract IInventory Inventory { get; }
        public IItemStackCollection Items => Inventory.Items;

        public event UnityAction OnValueChanged = delegate { };
        public event UnityAction<IPersistent> OnPersistentDataChanged = delegate { };

        protected virtual void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;            
        }

        public bool Add(IItem _item, int _quantity)
        {
            return Inventory.Add(_item, _quantity);
        }

        public bool Remove(IItem _item, int _quantity)
        {
            return Inventory.Remove(_item, _quantity);
        }        

        public bool Contains(IItem _item)
        {
            return Inventory.Contains(_item);
        }

        public int Quantity(IItem _item)
        {
            return Inventory.Quantity(_item);
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

        protected void TriggerOnPersistentDataChanged()
        {
            OnPersistentDataChanged?.Invoke(this);
        }

        protected virtual void OnDestroy()
        {
            if (Inventory != null) { Inventory.OnValueChanged -= TriggerOnValueChanged; }            
        }

        public virtual void Reset()
        {
            
        }

        public abstract void Load(ILoader _loader);
        public abstract void Save(ISaver _saver);        
    }
}
