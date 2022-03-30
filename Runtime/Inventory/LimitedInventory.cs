using System.Collections.Generic;

namespace Elysium.Items
{
    public class LimitedInventory : Inventory
    {
        protected LimitedItemStackCollection limitedItemStack => items as LimitedItemStackCollection;
        public int AvailableExpansion =>  limitedItemStack.Capacity.Max - limitedItemStack.Capacity.Value;
        public int AvailableShrink => limitedItemStack.Capacity.Value - limitedItemStack.Capacity.Min;

        public LimitedInventory(Capacity _config)
        {
            items = new LimitedItemStackCollection(_config);
            items.OnItemsChanged += TriggerOnItemsChanged;
            items.OnValueChanged += TriggerOnValueChanged;
        }

        public bool Expand(int _quantity)
        {            
            return limitedItemStack.Expand(_quantity);
        }

        public bool Shrink(int _quantity, out IEnumerable<IItemStack> _excessItems)
        {
            return limitedItemStack.Shrink(_quantity, out _excessItems);
        }
    }
}
