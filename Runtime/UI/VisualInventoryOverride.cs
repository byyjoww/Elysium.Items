using UnityEngine;

namespace Elysium.Items.UI
{
    public class VisualInventoryOverride : VisualInventory
    {
        public VisualInventoryOverride(VisualInventoryConfig _config, IInventoryTooltip _tooltip, IInventoryFilterer _filter, IInventorySlotView _view, GameObject _inventoryPanel) : base(_config, _tooltip, _filter, _view, _inventoryPanel)
        {

        }

        protected override void ConfigureSlot(IVisualInventorySlot _slot, IItemStack _stack)
        {
            _slot.Setup(new VisualInventorySlotConfig
            {
                Inventory = inventory,
                Stack = _stack,
                Event = useItemEvent,
            });
        }
    }
}