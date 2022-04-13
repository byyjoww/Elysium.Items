namespace Elysium.Items.UI
{
    public class VisualInventorySlotConfig
    {
        public IItemStack Stack { get; set; }
        public IUseItemEvent Event { get; set; }
        public bool CanSwap { get; set; }
    }
}