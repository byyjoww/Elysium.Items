namespace Elysium.Items.UI
{
    public class VisualInventorySlotConfig
    {
        public IInventory Inventory { get; set; }
        public IItemStack Stack { get; set; }
        public IUseItemEvent Event { get; set; }
    }
}