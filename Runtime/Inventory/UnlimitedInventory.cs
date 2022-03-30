namespace Elysium.Items
{
    public class UnlimitedInventory : Inventory
    {
        public UnlimitedInventory()
        {
            items = new UnlimitedItemStackCollection();
            items.OnItemsChanged += TriggerOnItemsChanged;
            items.OnValueChanged += TriggerOnValueChanged;
        }
    }
}
