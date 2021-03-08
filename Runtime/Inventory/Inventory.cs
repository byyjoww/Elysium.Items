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
    [CreateAssetMenu(fileName = "InventorySO", menuName = "Scriptable Objects/Item/Inventory")]
    public class Inventory : ScriptableObject, ISavable
    {
        [SerializeField] [Range(0, 100)] private int capacity = 20;
        [SerializeField] private List<ItemStack> itemStacks = new List<ItemStack>();
        [SerializeField] private List<ItemStack> defaultItemStacks = new List<ItemStack>();
        [SerializeField] private IInventoryElementDatabase inventoryElementsDatabase;

        public int Capacity => capacity;
        public int Used => itemStacks.Count;
        public int Free => capacity - itemStacks.Count;
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

            if (!HasRequiredSpace(_amount, _item.IsStackable, out int _required))
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

        public bool Remove(IInventoryElement _item, int _amount)
        {
            if (_amount < 0) { throw new ArgumentException($"Invalid value for {_amount}. Value must be a positive integer."); }

            if (!HasRequiredItems(_item, _amount))
            {
                Debug.Log($"Player has insufficient {_item.ItemName}.");
                return false;
            }

            RemoveItemsUntilDone(_item, _amount);

            Debug.Log($"Player lost x{_amount} {_item.ItemName}.");
            OnLost?.Invoke(_item, _amount);
            OnValueChanged?.Invoke();
            return true;
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
            defaultItemStacks = inventoryElementsDatabase.Elements.Select(x => new ItemStack(x as IInventoryElement, 0)).ToList();
        }

        [ContextMenu("Trigger OnValueChanged")]
        private void TriggerOnValueChanged()
        {
            OnValueChanged?.Invoke();
        }

        private bool HasRequiredSpace(int _amount, bool _isStackable, out int _required)
        {
            int requiredSpace = _isStackable ? 1 : _amount;
            _required = requiredSpace;
            return Free >= requiredSpace;
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
                itemStacks.Add(new ItemStack(_item, _amount));
                return;
            }

            for (int i = 0; i < _amount; i++)
            {
                itemStacks.Add(new ItemStack(_item, 1));
            }
        }

        private bool HasRequiredItems(IInventoryElement _item, int _amount)
        {
            return Quantity(_item) >= _amount;
        }

        private void RemoveItemsUntilDone(IInventoryElement _item, int _amount)
        {
            void Deduct(int _quantity)
            {
                ItemStack slot = GetFirstInventorySlotContaining(_item);
                int prev = slot.Amount;
                slot.Amount -= _quantity;

                int quantityEffectivelyDeducted = prev - Mathf.Max(slot.Amount, 0);
                _amount -= quantityEffectivelyDeducted;

                if (slot.Amount <= 0) { itemStacks.Remove(slot); }
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
            itemStacks = new List<ItemStack>();
            int size = reader.ReadInt32();

            for (int i = 0; i < size; i++)
            {
                string name = reader.ReadString();
                int amount = reader.ReadInt32();

                IInventoryElement item = inventoryElementsDatabase.GetElementByName(name);
                itemStacks.Add(new ItemStack(item, amount));
            }
        }

        public void LoadDefault()
        {
            Debug.Log($"OnLoad: loading default values for inventory {name}");
            if (defaultItemStacks == null) { defaultItemStacks = new List<ItemStack>(); }
            itemStacks = new List<ItemStack>(defaultItemStacks);
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
#if UNITY_EDITOR
            EditorPlayStateNotifier.RegisterOnExitPlayMode(this, () =>
            {
                // Debug.Log($"OnPlayModeExit: resetting inventory {name}");
                itemStacks = new List<ItemStack>();
            });
#endif
        }
    }
}