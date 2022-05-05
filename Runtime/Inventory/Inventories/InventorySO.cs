using Elysium.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items
{
    public abstract class InventorySO : ScriptableObject, IInventory, IPersistent
    {
        public Guid InventoryID { get; protected set; }
        public abstract IItemStackCollection Items { get; }        

        public event UnityAction OnValueChanged = delegate { };
        public event UnityAction<IPersistent> OnPersistentDataChanged = delegate { };

        protected virtual void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
            Items.OnValueChanged.AddListener(TriggerOnValueChanged);
            Items.OnValueChanged.AddListener(TriggerOnPersistentDataChanged);
        }

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

        public IEnumerator<IItemStack> GetEnumerator()
        {
            return Items.GetEnumerator();
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

        protected virtual void OnDisable()
        {
            Items.OnValueChanged.RemoveAllListeners();
        }

        protected virtual void OnValidate()
        {
            
        }

        public virtual void Reset()
        {
            //EditorPlayStateNotifier.RegisterOnExitPlayMode(this, () =>
            //{
            //    items.Empty();
            //});
        }

        public abstract void Load(ILoader _loader);
        public abstract void Save(ISaver _saver);        
    }
}
