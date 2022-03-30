namespace Elysium.Items
{
    public class UnlimitedInventory : Inventory
    {
        public UnlimitedInventory()
        {
            items = new UnlimitedItemStackCollection();
        }
    }
}
