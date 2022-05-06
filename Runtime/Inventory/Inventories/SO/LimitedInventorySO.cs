using Elysium.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    [CreateAssetMenu(fileName = "LimitedInventorySO_", menuName = "Scriptable Objects/Items/Inventory/Limited")]
    public class LimitedInventorySO : InventorySO, IExpandable
    {
        [SerializeField] private Capacity capacity = default;
        private PersistentLimitedInventory inventory = default;

        public int AvailableExpansion => inventory.AvailableExpansion;
        public int AvailableShrink => inventory.AvailableShrink;
        protected override IInventory Inventory => LimitedInventory;
        protected PersistentLimitedInventory LimitedInventory
        {
            get
            {
                if (inventory == null)
                {
                    inventory = PersistentLimitedInventory.New(inventoryID, capacity);
                    inventory.OnValueChanged += TriggerOnValueChanged;
                }
                return inventory;
            }
        }        

        public static LimitedInventorySO New(Capacity _capacity)
        {
            LimitedInventorySO instance = CreateInstance<LimitedInventorySO>();
            instance.capacity = _capacity;
            return instance;
        }

        public static LimitedInventorySO New(Guid _inventoryID, Capacity _capacity)
        {
            LimitedInventorySO instance = CreateInstance<LimitedInventorySO>();
            instance.inventoryID = _inventoryID;
            instance.capacity = _capacity;
            return instance;
        }

        public bool Expand(int _quantity)
        {
            return LimitedInventory.Expand(_quantity);
        }

        public bool Shrink(int _quantity, out IEnumerable<IItemStack> _excessItems)
        {
            return LimitedInventory.Shrink(_quantity, out _excessItems);
        }

        public override void Load(ILoader _loader)
        {
            LimitedInventory.Load(_loader);
        }

        public override void Save(ISaver _saver)
        {
            LimitedInventory.Save(_saver);
        }

        public override void Reset()
        {
            if (inventory != null) { inventory.OnValueChanged -= TriggerOnValueChanged; }
            inventory = null;
            base.Reset();
        }
    }
}
