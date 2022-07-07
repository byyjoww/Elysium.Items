using UnityEngine;

namespace Elysium.Items.UI
{
    public class InventoryPresenterOverride : InventoryPresenter
    {
        public InventoryPresenterOverride(InventoryPresenterConfig _config, IItemFilterer _filter, IInventoryViewSlotSpawner _view, GameObject _inventoryPanel) : base(_config, _filter, _view, _inventoryPanel)
        {

        }

        protected override void ConfigureSlot(IInventoryViewSlot _slot, IItemStack _stack)
        {
            _slot.Setup(new IInventoryViewSlotConfig
            {
                Stack = _stack,
                Event = useItemEvent,
                CanSwap = false,
            });
        }
    }
}