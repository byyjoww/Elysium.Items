using System.Collections.Generic;

namespace Elysium.Items
{
    public class LimitedInventory : Inventory
    {
        protected LimitedItemStackCollection limitedItemStack => items as LimitedItemStackCollection;

        public LimitedInventory(LimitedItemStackCollection.Config _config)
        {
            items = new LimitedItemStackCollection(_config);
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
