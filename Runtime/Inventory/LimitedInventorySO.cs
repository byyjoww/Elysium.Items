using Elysium.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    [CreateAssetMenu(fileName = "LimitedInventorySO_", menuName = "Scriptable Objects/Inventory/Limited Inventory")]
    public class LimitedInventorySO : InventorySO, IExpandable
    {
        [SerializeField] private Capacity capacity = new Capacity();
        protected LimitedItemStackCollection items = default;

        public override IItemStackCollection Items => items;
        public int AvailableExpansion => items.Capacity.Max - items.Capacity.Value;
        public int AvailableShrink => items.Capacity.Value - items.Capacity.Min;

        protected override void OnEnable()
        {
            items = new LimitedItemStackCollection(new List<IItemStack>(), capacity, capacity.Default);
            base.OnEnable();
        }

        public static LimitedInventorySO New(Capacity _capacity)
        {
            var inventory = CreateInstance<LimitedInventorySO>();
            inventory.InventoryID = Guid.NewGuid();
            inventory.capacity = _capacity;
            inventory.Rebind(new List<IItemStack>(), _capacity.Default);
            return inventory;
        }

        public static LimitedInventorySO New(Guid _inventoryID, Capacity _capacity)
        {
            var inventory = CreateInstance<LimitedInventorySO>();
            inventory.InventoryID = _inventoryID;
            inventory.capacity = _capacity;
            inventory.Rebind(new List<IItemStack>(), _capacity.Default);
            return inventory;
        }

        public bool Expand(int _quantity)
        {
            return items.Expand(_quantity);
        }

        public bool Shrink(int _quantity, out IEnumerable<IItemStack> _excessItems)
        {
            return items.Shrink(_quantity, out _excessItems);
        }

        public override void Load(ILoader _loader)
        {
            var save = _loader.LoadPersistency(InventoryID.ToString());
            var itemDictionary = save.LoadDictionary("items");
            var items = new List<IItemStack>();
            foreach (var item in itemDictionary)
            {
                IItemStack stack = new ItemStack(new GenericItem(item.Key, item.Key), Convert.ToInt32(item.Value));
                items.Add(stack);
                Debug.Log($"loaded stack {stack.ToString()}");
            }
            var capacity = save.LoadInt("capacity");
            Rebind(items, capacity);
        }

        public override void Save(ISaver _saver)
        {
            IPersistency save = _saver.CreatePersistency(InventoryID.ToString());
            var itemDictionary = new Dictionary<string, object>();
            foreach (var stack in Items.Stacks)
            {
                if (stack.IsEmpty) { continue; }
                itemDictionary.Add(stack.Item.ItemID.ToString(), stack.Quantity);
                Debug.Log($"saved stack {stack.ToString()}");
            }
            save.Write("items", itemDictionary);
            save.Write("capacity", items.Capacity.Value);
            save.Commit();
        }

        private void Rebind(IEnumerable<IItemStack> _stacks, int _currentCapacity)
        {
            Items.OnValueChanged.RemoveAllListeners();
            items = new LimitedItemStackCollection(_stacks, capacity, _currentCapacity);
            Items.OnValueChanged.AddListener(TriggerOnValueChanged);
            Items.OnValueChanged.AddListener(TriggerOnPersistentDataChanged);
        }

        protected override void OnValidate()
        {
            Reset();
        }

        public override void Reset()
        {
            items = new LimitedItemStackCollection(new List<IItemStack>(), capacity, capacity.Default);
            base.OnEnable();
        }
    }
}
