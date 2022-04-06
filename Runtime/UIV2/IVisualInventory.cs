using System;
using System.Collections;

namespace Elysium.Items.UI
{
    public interface IVisualInventory
    {
        void Open(IInventory _inventory, IItemFilterConfig _config, IUseItemEvent _event);
    }
}