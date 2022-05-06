using Elysium.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items
{
    public class PersistentLimitedInventory : PersistentInventory, IExpandable
    {
        private Capacity capacity = default;
        private LimitedInventory inventory = default;

        protected override IInventory Inventory => inventory;
        public int AvailableExpansion => inventory.AvailableExpansion;
        public int AvailableShrink => inventory.AvailableShrink;        

        public static PersistentLimitedInventory New(Guid _inventoryID, Capacity _capacity)
        {
            return new PersistentLimitedInventory(_inventoryID, _capacity);
        }

        protected PersistentLimitedInventory(Guid _inventoryID, Capacity _capacity) : base(_inventoryID)
        {
            this.capacity = _capacity;
        }

        public bool Expand(int _quantity)
        {
            return inventory.Expand(_quantity);
        }

        public bool Shrink(int _quantity, out IEnumerable<IItemStack> _excessItems)
        {
            return inventory.Shrink(_quantity, out _excessItems);
        }

        public override void Load(ILoader _loader)
        {
            var save = _loader.LoadPersistency(inventoryID.ToString());
            if (save.IsDefault)
            {
                CreateDefaultInventory();
                return;
            }

            var itemDictionary = save.LoadArray("items");
            var items = new List<IItemStack>();
            foreach (var item in itemDictionary)
            {
                IItemStack stack = ItemStack.WithContents(new GenericItem(item.Key, item.Key), Convert.ToInt32(item.Value));
                items.Add(stack);
                Debug.Log($"loaded stack {stack.ToString()}");
            }
            int currentCapacity = save.LoadInt("capacity");
            CreateLoadedInventory(items, currentCapacity);
        }

        public override void Save(ISaver _saver)
        {
            IPersistency save = _saver.CreatePersistency(inventoryID.ToString());
            var array = new List<KeyValuePair<string, object>>();
            foreach (var stack in Items.Stacks)
            {
                if (stack.IsEmpty) { continue; }
                array.Add(new KeyValuePair<string, object>(stack.Item.ItemID.ToString(), stack.Quantity));
                Debug.Log($"saved stack {stack.ToString()}");
            }
            save.Write("items", array);
            save.Write("capacity", inventory.NumOfSlots);
            save.Commit();
        }

        private void CreateDefaultInventory()
        {
            inventory = LimitedInventory.New(capacity);
            inventory.OnValueChanged += TriggerOnValueChanged;
        }

        private void CreateLoadedInventory(List<IItemStack> items, int currentCapacity)
        {
            if (inventory != null) { inventory.OnValueChanged -= TriggerOnValueChanged; }
            inventory = LimitedInventory.New(capacity, currentCapacity, items);
            inventory.OnValueChanged += TriggerOnValueChanged;
        }
    }
}
