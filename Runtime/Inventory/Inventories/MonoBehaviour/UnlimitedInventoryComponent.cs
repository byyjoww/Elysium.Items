using Elysium.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    public class UnlimitedInventoryComponent : InventoryComponent
    {
        private PersistentUnlimitedInventory inventory = default;

        protected override IInventory Inventory => inventory;

        protected override void Awake()
        {
            inventory = PersistentUnlimitedInventory.New(inventoryID);
            base.Awake();
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
