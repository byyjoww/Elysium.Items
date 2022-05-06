using Elysium.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    [CreateAssetMenu(fileName = "UnlimitedInventorySO_", menuName = "Scriptable Objects/Items/Inventory/Unlimited")]
    public class UnlimitedInventorySO : InventorySO
    {
        private PersistentUnlimitedInventory inventory = default;

        protected override IInventory Inventory => PersistentInventory;
        protected PersistentUnlimitedInventory PersistentInventory
        {
            get
            {
                if (inventory == null)
                {
                    inventory = PersistentUnlimitedInventory.New(inventoryID);
                    PersistentInventory.OnValueChanged += TriggerOnValueChanged;
                }
                return inventory;
            }
        }

        public static UnlimitedInventorySO New()
        {
            return CreateInstance<UnlimitedInventorySO>();
        }

        public static UnlimitedInventorySO New(Guid _inventoryID)
        {
            UnlimitedInventorySO instance = CreateInstance<UnlimitedInventorySO>();
            instance.inventoryID = _inventoryID;
            return instance;
        }

        public override void Load(ILoader _loader)
        {
            PersistentInventory.Load(_loader);
        }

        public override void Save(ISaver _saver)
        {
            PersistentInventory.Save(_saver);
        }

        public override void Reset()
        {
            if (inventory != null) { inventory.OnValueChanged -= TriggerOnValueChanged; }
            inventory = null;
            base.Reset();
        }
    }
}
