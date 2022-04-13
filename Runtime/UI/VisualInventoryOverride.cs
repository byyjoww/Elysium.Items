using UnityEngine;

namespace Elysium.Items.UI
{
    public class VisualInventoryOverride : VisualInventory
    {
        public VisualInventoryOverride(VisualInventoryConfig _config, IItemFilterer _filter, IInventorySlotView _view, GameObject _inventoryPanel) : base(_config, _filter, _view, _inventoryPanel)
        {

        }

        protected override void ConfigureSlot(IVisualInventorySlot _slot, IItemStack _stack)
        {
            _slot.Setup(new VisualInventorySlotConfig
            {
                Stack = _stack,
                Event = useItemEvent,
                CanSwap = false,
            });
        }
    }
}