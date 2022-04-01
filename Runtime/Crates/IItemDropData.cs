namespace Elysium.Items
{
    public interface IItemDropData
    {
        IItem Item { get; }
        float Chance { get; }
    }
}
