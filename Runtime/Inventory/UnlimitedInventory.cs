using Elysium.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items
{
    public class UnlimitedInventory : Inventory
    {
        protected UnlimitedItemStackCollection items = default;

        public override IItemStackCollection Items => items;

        private UnlimitedInventory(IEnumerable<IItemStack> _stacks)
        {
            items = new UnlimitedItemStackCollection(_stacks);
            items.OnValueChanged.AddListener(TriggerOnValueChanged);
        }

        public static UnlimitedInventory New()
        {
            return new UnlimitedInventory(new List<IItemStack>());
        }

        public static UnlimitedInventory New(IEnumerable<IItemStack> _stacks)
        {
            return new UnlimitedInventory(_stacks);
        }
    }
}
