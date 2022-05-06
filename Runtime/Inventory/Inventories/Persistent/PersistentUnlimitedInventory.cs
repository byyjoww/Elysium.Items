using Elysium.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Items
{
    public class PersistentUnlimitedInventory : PersistentInventory
    {
        private UnlimitedInventory inventory = default;

        protected override IInventory Inventory => inventory;

        public static PersistentUnlimitedInventory New(Guid _inventoryID)
        {
            return new PersistentUnlimitedInventory(_inventoryID);
        }

        protected PersistentUnlimitedInventory(Guid _inventoryID) : base(_inventoryID)
        {
            
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
            CreateLoadedInventory(items);
        }

        public override void Save(ISaver _saver)
        {
            IPersistency save = _saver.CreatePersistency(inventoryID.ToString());
            var array = new List<KeyValuePair<string, object>>();
            foreach (var stack in Items.Stacks)
            {
                array.Add(new KeyValuePair<string, object>(stack.Item.ItemID.ToString(), stack.Quantity));
                Debug.Log($"saved stack {stack.ToString()}");
            }
            save.Write("items", array);
            save.Commit();
        }

        private void CreateDefaultInventory()
        {
            inventory = UnlimitedInventory.New();
            inventory.OnValueChanged += TriggerOnValueChanged;
        }

        private void CreateLoadedInventory(List<IItemStack> items)
        {
            inventory = UnlimitedInventory.New(items);
            inventory.OnValueChanged += TriggerOnValueChanged;
        }
    }
}
