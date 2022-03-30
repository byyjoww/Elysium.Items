using Elysium.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items
{
    [CreateAssetMenu(fileName = "InventorySO_", menuName = "Scriptable Objects/Inventory/Inventory")]
    public class InventorySO : ScriptableObject, IInventory
    {
        public IItemStackCollection Items => throw new NotImplementedException();

        public event UnityAction OnValueChanged = delegate { };
        public event UnityAction OnItemsChanged = delegate { };

        public bool Add(IItem _item, int _quantity)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IItem _item, int _quantity)
        {
            throw new NotImplementedException();
        }        

        public bool Contains(IItem _item)
        {
            return Items.Contains(_item);
        }

        public int Quantity(IItem _item)
        {
            throw new NotImplementedException();
        }

        public void Swap(IItemStack _origin, IItemStack _destination)
        {
            throw new NotImplementedException();
        }

        public void Empty()
        {
            throw new NotImplementedException();
        }

        //[System.Serializable]
        //public class Config
        //{
        //    [SerializeField] private bool resizable = false;

        //    public bool Resizable => resizable;
        //}

        //[SerializeField] private Config config = default;
        //[SerializeReference] private IItemStackCollection items = new LimitedItemStackCollection();

        //public ushort Size => items.Size;
        //public IItemStackCollection Items => items;

        //public event UnityAction OnItemsChanged = delegate { };
        //public event UnityAction OnValueChanged = delegate { };

        //public bool Add(IItem _item, int _quantity) => items.Add(_item, _quantity);

        //public bool Remove(IItem _item, int _quantity) => items.Remove(_item, _quantity);

        //public int Quantity(IItem _item) => items.Quantity(_item);        

        //public void Empty() => items.Empty();

        //public void Swap(IItemStack _origin, IItemStack _destination)
        //{
        //    IItem item = _destination.Item;
        //    int quantity = _destination.Quantity;

        //    _destination.Set(_origin.Item, _origin.Quantity);
        //    _origin.Set(item, quantity);
        //    OnValueChanged?.Invoke();
        //}

        //public void Load(BinaryReader _reader) => items.Load(_reader);

        //public void LoadDefault() => items.LoadDefault();

        //public void Save(BinaryWriter _writer) => items.Save(_writer);

        //private void OnEnable()
        //{
        //    hideFlags = HideFlags.DontUnloadUnusedAsset;
        //    items.OnValueChanged += TriggerOnValueChanged;
        //    items.OnItemsChanged += TriggerOnItemsChanged;
        //}

        //private void OnDisable()
        //{
        //    items.OnValueChanged -= TriggerOnValueChanged;
        //    items.OnItemsChanged -= TriggerOnItemsChanged;
        //}

        //private void TriggerOnValueChanged()
        //{
        //    OnValueChanged?.Invoke();
        //}

        //private void TriggerOnItemsChanged()
        //{
        //    OnItemsChanged?.Invoke();
        //}

        //private void OnValidate()
        //{
        //    if (config.Resizable && items.GetType() != typeof(UnlimitedItemStackCollection))
        //    {
        //        items = new UnlimitedItemStackCollection();
        //    }

        //    if (!config.Resizable && items.GetType() != typeof(LimitedItemStackCollection))
        //    {
        //        items = new LimitedItemStackCollection();
        //    }

        //    items.OnValidate();
        //    EditorPlayStateNotifier.RegisterOnExitPlayMode(this, () =>
        //    {
        //        items.Empty();
        //    });
        //}
    }
}
