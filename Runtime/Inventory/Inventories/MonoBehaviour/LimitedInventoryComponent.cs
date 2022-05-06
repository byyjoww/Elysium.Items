using Elysium.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    public class LimitedInventoryComponent : InventoryComponent, IExpandable
    {
        [SerializeField] private Capacity capacity = default;
        private PersistentLimitedInventory inventory = default;

        protected override IInventory Inventory => inventory;
        public int AvailableExpansion => inventory.AvailableExpansion;
        public int AvailableShrink => inventory.AvailableShrink;

        protected override void Awake()
        {
            inventory = PersistentLimitedInventory.New(inventoryID, capacity);
            base.Awake();
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
            inventory.Load(_loader);
        }

        public override void Save(ISaver _saver)
        {
            inventory.Save(_saver);
        }
    }
}
