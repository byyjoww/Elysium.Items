using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Events;
using Elysium.Utils.Attributes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Elysium.Core;
using Elysium.Utils;

namespace Elysium.Items
{
    [CreateAssetMenu(fileName = "InventorySO", menuName = "Scriptable Objects/Items/InventorySO")]
    public class InventorySO : ScriptableObject, ISavable
    {
        [Separator("Settings", true)]
        [SerializeField] private bool resizable = false;
        [SerializeField] [Range(0, 100)] private int capacity = 20;
        
        [Separator("Current Items", true)]
        [SerializeField] private ItemStack[] itemStacks = new ItemStack[20];

        [Separator("Default Items", true)]
        [SerializeField] private ItemStack[] defaultItemStacks = new ItemStack[20];

        [Separator("All Items", true)]
        [SerializeField] private IInventoryElementDatabase inventoryElementsDatabase;

        public int Capacity => capacity;
        public int Used => itemStacks.Where(x => !x.IsNullOrEmpty()).Count();
        public int Free => capacity - Used;
        public IReadOnlyList<ItemStack> ItemStacks => itemStacks;

        public event UnityAction<IInventoryElement, int> OnGained;
        public event UnityAction<IInventoryElement, int> OnLost;
        public event UnityAction OnValueChanged;

        private void OnEnable() => hideFlags = HideFlags.DontUnloadUnusedAsset;

        public bool Contains(IInventoryElement _item)
        {
            ItemStack stack = GetFirstInventorySlotContaining(_item);
            if (stack == null) { return false; }
            return true;
        }

        public int Quantity(IInventoryElement _item)
        {
            int quantity = 0;
            IEnumerable<ItemStack> allStacks = itemStacks.Where(x => x.InventoryElement == _item);

            foreach (var stack in allStacks)
            {
                quantity += stack.Amount;
            }

            return quantity;
        }

        public ItemStack GetFirstInventorySlotContaining(IInventoryElement _item)
        {
            return itemStacks.FirstOrDefault(x => x.InventoryElement == _item);
        }

        public bool Add(IInventoryElement _item, int _amount)
        {
            if (_amount < 0) { throw new ArgumentException($"Invalid value for {_amount}. Value must be a positive integer."); }

            if (!HasRequiredSpace(_item, _amount, _item.IsStackable, out int _required))
            {
                Debug.LogError($"Not enough space in inventory! | Space: {Free} | Required: {_required}");
                return false;
            }

            if (!TryAddToExistingStack(_item, _amount))
            {
                AddToNewStack(_item, _amount);
            }

            Debug.Log($"Gained x{_amount} {_item.ItemName}.");
            OnGained?.Invoke(_item, _amount);
            OnValueChanged?.Invoke();
            return true;
        }

        public bool Remove(IInventoryElement _item, int _amount, ItemStack _preferredStack = null)
        {
            if (_amount < 0) { throw new ArgumentException($"Invalid value for {_amount}. Value must be a positive integer."); }

            if (!HasRequiredItems(_item, _amount))
            {
                Debug.Log($"Player has insufficient {_item.ItemName}.");
                return false;
            }

            RemoveItemsUntilDone(_item, _amount, _preferredStack);

            Debug.Log($"Player lost x{_amount} {_item.ItemName}.");
            OnLost?.Invoke(_item, _amount);
            OnValueChanged?.Invoke();
            return true;
        }

        public void SwapStackContents(ItemStack _origin, ItemStack _destination)
        {
            IInventoryElement item = _destination.InventoryElement;
            int amount = _destination.Amount;

            _destination.SetContents(_origin.InventoryElement, _origin.Amount);
            _origin.SetContents(item, amount);
            OnValueChanged?.Invoke();
        }

        public T[] GetAllItemsOfType<T>() where T : ScriptableObject
        {
            ItemStack[] typedStacks = itemStacks.Where(x => x != null && (x.Element.GetType() == typeof(T) || x.Element.GetType().IsAssignableFrom(typeof(T)))).ToArray();
            return typedStacks.Select(x => x.Element).Cast<T>().ToArray();
        }

        public T GetItemByScriptableObjectName<T>(string _name) where T : ScriptableObject
        {
            T[] items = GetAllItemsOfType<T>();
            return items.SingleOrDefault(x => x.name == _name);
        }

        [ContextMenu("All To Default")]
        private void AddAllElementsToDefault()
        {
            defaultItemStacks = inventoryElementsDatabase.Elements
                .Select(x => ItemStack.WithContents(x as IInventoryElement, 1))
                .ToArray();
        }

        [ContextMenu("Trigger OnValueChanged")]
        private void TriggerOnValueChanged()
        {
            OnValueChanged?.Invoke();
        }

        private bool HasRequiredSpace(IInventoryElement _item, int _amount, bool _isStackable, out int _requiredSpace)
        {
            int requiredStackableSpace = Contains(_item) ? 0 : 1;
            _requiredSpace = _isStackable ? requiredStackableSpace : _amount;

            if (resizable && Free < _requiredSpace && (itemStacks.Length + _requiredSpace - Free) < Capacity) 
            {
                Array.Resize(ref itemStacks, itemStacks.Length + _requiredSpace - Free); 
            }

            return Free >= _requiredSpace;
        }

        private bool TryAddToExistingStack(IInventoryElement _item, int _amount)
        {
            if (!_item.IsStackable) { return false; }

            ItemStack slot = GetFirstInventorySlotContaining(_item);
            if (slot == null) { return false; }
            slot.Amount += _amount;
            return true;
        }

        private void AddToNewStack(IInventoryElement _item, int _amount)
        {            
            if (_item.IsStackable)
            {
                ItemStack slot = itemStacks.First(x => x.Element == null);
                slot.SetContents(_item, _amount);
                return;
            }

            for (int i = 0; i < _amount; i++)
            {
                ItemStack slot = itemStacks.First(x => x.Element == null);
                slot.SetContents(_item, 1);
            }
        }

        private bool HasRequiredItems(IInventoryElement _item, int _amount)
        {
            return Quantity(_item) >= _amount;
        }

        private void RemoveItemsUntilDone(IInventoryElement _item, int _amount, ItemStack _preferredStack = null)
        {
            if (!_preferredStack.IsNullOrEmpty() && _preferredStack.InventoryElement != _item) { throw new System.Exception($"preferred stack's item {_preferredStack.InventoryElement.ItemName} doesnt match item to be removed {_item.ItemName}"); }

            void Deduct(int _quantity)
            {
                ItemStack slot = _preferredStack;
                if (slot.IsNullOrEmpty())
                {
                    slot = GetFirstInventorySlotContaining(_item);
                }

                int prev = slot.Amount;
                slot.Amount -= _quantity;

                int quantityEffectivelyDeducted = prev - Mathf.Max(slot.Amount, 0);
                _amount -= quantityEffectivelyDeducted;

                if (slot.Amount <= 0) { slot.Element = null; }
            }

            while (_amount > 0) { Deduct(_amount); }
        }

        // ---------------------------------- ISavable ---------------------------------- //

        public ushort Size
        {
            get
            {
                var items = itemStacks.Where(x => x.Element != null).Select(x => new Tuple<string, int>(x.Element.name, x.Amount)).ToArray();
                Stream stream = new MemoryStream();
                IFormatter formatter = new BinaryFormatter();

                formatter.Serialize(stream, items);
                ushort length = (ushort)stream.Length;
                stream.Dispose();

                return length;
            }
        }

        public void Load(BinaryReader reader)
        {
            int size = reader.ReadInt32();
            itemStacks = new ItemStack[size];

            for (int i = 0; i < size; i++)
            {
                string name = reader.ReadString();
                int amount = reader.ReadInt32();

                IInventoryElement item = inventoryElementsDatabase.GetElementByName(name);
                AddToNewStack(item, amount);
            }
        }

        public void LoadDefault()
        {
            Debug.Log($"OnLoad: loading default values for inventory {name}");
            if (defaultItemStacks == null) { defaultItemStacks = new ItemStack[Capacity]; }
            Array.Copy(defaultItemStacks, itemStacks, Capacity);
            OnValueChanged?.Invoke();
        }

        public void Save(BinaryWriter writer)
        {
            Tuple<string, int>[] items = itemStacks.Where(x => x.Element != null).Select(x => new Tuple<string, int>(x.Element.name, x.Amount)).ToArray();
            writer.Write(items.Length);

            for (int i = 0; i < items.Length; i++)
            {
                // WRITE THE NAME OF THE ITEM
                writer.Write(items[i].Item1);
                // WRITE NUMBER OF ITEMS
                writer.Write(items[i].Item2);
            }
        }

        private void OnValidate()
        {
            inventoryElementsDatabase.Refresh();

            if (!resizable)
            {
                if (defaultItemStacks.Length != Capacity) { Array.Resize(ref defaultItemStacks, Capacity); }
                if (itemStacks.Length != Capacity) { Array.Resize(ref itemStacks, Capacity); }
            }

#if UNITY_EDITOR
            EditorPlayStateNotifier.RegisterOnExitPlayMode(this, () =>
            {
                // Debug.Log($"OnPlayModeExit: resetting inventory {name}");
                itemStacks = new ItemStack[Capacity];
            });
#endif
        }
    }
}