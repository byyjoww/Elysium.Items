using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Elysium.Items
{
    public class NullInventory : IInventory
    {
        public IItemStackCollection Items => new NullItemStackCollection();

        public event UnityAction OnValueChanged = delegate { };

        public bool Add(IItem _item, int _quantity)
        {
            return false;
        }

        public bool Contains(IItem _item)
        {
            return false;
        }

        public void Empty()
        {
            
        }

        public IEnumerator<IItemStack> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Quantity(IItem _item)
        {
            return 0;
        }

        public bool Remove(IItem _item, int _quantity)
        {
            return false;
        }        
    }
}