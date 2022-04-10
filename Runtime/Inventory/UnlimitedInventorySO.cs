using Elysium.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    [CreateAssetMenu(fileName = "UnlimitedInventorySO_", menuName = "Scriptable Objects/Items/Inventory/Unlimited")]
    public class UnlimitedInventorySO : InventorySO
    {
        protected UnlimitedItemStackCollection items = default;

        public override IItemStackCollection Items => items;

        protected override void OnEnable()
        {
            items = new UnlimitedItemStackCollection(new List<IItemStack>());
            base.OnEnable();
        }

        public static UnlimitedInventorySO New()
        {
            var inventory = CreateInstance<UnlimitedInventorySO>();
            inventory.InventoryID = Guid.NewGuid();
            inventory.Rebind(new List<IItemStack>());
            return inventory;
        }

        public static UnlimitedInventorySO New(Guid _inventoryID)
        {
            var inventory = CreateInstance<UnlimitedInventorySO>();
            inventory.InventoryID = _inventoryID;
            inventory.Rebind(new List<IItemStack>());
            return inventory;
        }

        public override void Load(ILoader _loader)
        {
            var save = _loader.LoadPersistency(InventoryID.ToString());
            var itemDictionary = save.LoadDictionary("items");
            var items = new List<IItemStack>();
            foreach (var item in itemDictionary)
            {
                IItemStack stack = ItemStack.WithContents(new GenericItem(item.Key, item.Key), Convert.ToInt32(item.Value));
                items.Add(stack);
                Debug.Log($"loaded stack {stack.ToString()}");
            }
            Rebind(items);
        }

        public override void Save(ISaver _saver)
        {
            IPersistency save = _saver.CreatePersistency(InventoryID.ToString());
            var itemDictionary = new Dictionary<string, object>();
            foreach (var stack in Items.Stacks)
            {
                itemDictionary.Add(stack.Item.ItemID.ToString(), stack.Quantity);
                Debug.Log($"saved stack {stack.ToString()}");
            }
            save.Write("items", itemDictionary);
            save.Commit();
        }

        private void Rebind(IEnumerable<IItemStack> _stacks)
        {
            Items.OnValueChanged.RemoveAllListeners();
            items = new UnlimitedItemStackCollection(_stacks);
            Items.OnValueChanged.AddListener(TriggerOnValueChanged);
            Items.OnValueChanged.AddListener(TriggerOnPersistentDataChanged);
        }

        protected override void OnValidate()
        {
            Reset();
        }

        public override void Reset()
        {
            items = new UnlimitedItemStackCollection(new List<IItemStack>());
            base.OnEnable();
        }
    }
}
