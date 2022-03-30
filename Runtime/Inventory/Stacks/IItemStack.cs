namespace Elysium.Items
{
    public interface IItemStack
    {
        string ID { get; }
        bool IsEmpty { get; }
        bool IsFull { get; }
        IItem Item { get; }
        int Quantity { get; }

        void Add(int _quantity);
        void Empty();
        void Remove(int _quantity);
        void Set(IItem _item);
        void Set(IItem _item, int _value);
        void Set(int _value);
    }
}
