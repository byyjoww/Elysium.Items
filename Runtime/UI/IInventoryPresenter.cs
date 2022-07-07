using System;
using System.Collections;

namespace Elysium.Items.UI
{
    public interface IInventoryPresenter
    {
        void Show(IInventory _inventory, IItemFilterConfig _config, IUseItemEvent _event);
        void Hide();
    }
}