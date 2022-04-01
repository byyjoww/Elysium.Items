using System;

namespace Elysium.Items
{
    public class NullItemStack : IItemStack
    {
        private IItem item = new NullItem();
        public Guid ID => Guid.Empty;
        public bool IsEmpty => true;
        public bool IsFull => false;
        public IItem Item => item;
        public int Quantity => 0;

        public void Add(int _quantity)
        {
            
        }

        public void Empty()
        {
            
        }

        public void Remove(int _quantity)
        {
            
        }

        public void Set(IItem _item)
        {
            
        }

        public void Set(IItem _item, int _value)
        {
            
        }

        public void Set(int _value)
        {
            
        }
    }
}
